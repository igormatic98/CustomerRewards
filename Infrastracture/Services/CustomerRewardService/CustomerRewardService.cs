using CustomerRewards.Infrastructure;

namespace Infrastracture.Services.CustomerRewardService;

using Auth.Services;
using AutoMapper;
using CustomerRewards.Catalog.Entities;
using CustomerRewards.Company.Entities;
using Infrastracture.Services.ExternalCustomerService;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Servis za dodjeljivanje poklon bona kupcima
/// Kupci se uzimaju iz eksternog apija, ili lokalnog u zavisnosti da li se nalaze u lokalnoj bazi
/// </summary>
public class CustomerRewardService
{
    private readonly DatabaseContext databaseContext;
    private readonly ExternalCustomerService externalCustomerService;
    private readonly IMapper mapper;
    private readonly TokenReaderService tokenReaderService;

    public CustomerRewardService(
        DatabaseContext databaseContext,
        ExternalCustomerService externalCustomerService,
        TokenReaderService tokenReaderService,
        IMapper mapper
    )
    {
        this.databaseContext = databaseContext;
        this.externalCustomerService = externalCustomerService;
        this.mapper = mapper;
        this.tokenReaderService = tokenReaderService;
    }

    public async Task GetExternalCustomer(int customerId, decimal rewardAmount)
    {
        var agentId = tokenReaderService.GetAgentId();
        var campaignId = tokenReaderService.GetCampaignId();
        var currentDate = DateTime.Now.Date;

        //provjeravamo da li je dati agent vec dodijelio 5 bonova na taj dan za tu kampanju
        var numberOfAddedCustomerByAgent = await databaseContext.CustomersRewards
            .Where(
                cr =>
                    cr.AgentId == agentId
                    && cr.CampaignId == campaignId
                    && cr.RewardDate.Date == currentDate
            )
            .CountAsync();

        if (numberOfAddedCustomerByAgent > 5)
        {
            throw new Exception(
                "Agent added already max(5) customer to reward on this campaign today."
            );
        }

        //provjeravamo da li je dati korisnik vec dobio poklon bon

        var isRewardAlradyGiven = await databaseContext.CustomersRewards.AnyAsync(
            cr => cr.Customer.ExternalId == customerId && cr.CampaignId == campaignId
        );
        if (isRewardAlradyGiven)
        {
            throw new Exception("Reward is already given to this customer.");
        }

        if (campaignId != 0)
        {
            var home = new Address();
            var office = new Address();
            var customer = new Customer();
            var transaction = await databaseContext.Database.BeginTransactionAsync();
            try
            {
                var isCustomerInLocalDB = await databaseContext.Customers.AnyAsync(
                    x => x.ExternalId == customerId
                );
                if (!isCustomerInLocalDB)
                {
                    //poziv eksternog apija da bi dobili podatke o kupcu, obzirom da se ne nalazi u lokalnoj bazi
                    var findPersonResult = await externalCustomerService.GetExternalCustomer(
                        customerId
                    );

                    if (findPersonResult != null)
                    {
                        //provjeravanmo da li postoji adresa u lokalnoj bazi
                        bool homeExists = await AddressExists(findPersonResult.Home);
                        if (!homeExists)
                        {
                            home = mapper.Map<Address>(findPersonResult.Home);
                            await databaseContext.Address.AddAsync(home);
                        }
                        else
                            home = await GetAdressFromDb(findPersonResult.Home);

                        // Proveri da li postoji office adresa u lokalnoj bazi
                        bool officeExists = await AddressExists(findPersonResult.Office);
                        if (!officeExists)
                        {
                            office = mapper.Map<Address>(findPersonResult.Office);
                            await databaseContext.Address.AddAsync(office);
                        }
                        else
                            office = await GetAdressFromDb(findPersonResult.Office);
                        await databaseContext.SaveChangesAsync();

                        customer = new Customer
                        {
                            ExternalId = customerId,
                            Age = findPersonResult.Age,
                            HomeId = home.Id,
                            OfficeId = office.Id,
                            Dob = findPersonResult.DOB,
                            Ssn = findPersonResult.SSN,
                            Name = findPersonResult.Name,
                        };
                        await databaseContext.Customers.AddAsync(customer);
                        await databaseContext.SaveChangesAsync();
                    }
                }
                else
                {
                    //kupac postoji u lokalnoj bazi, odakle ga povlacimo
                    customer = await databaseContext.Customers
                        .Where(c => c.ExternalId == customerId)
                        .FirstOrDefaultAsync();
                }

                //dodjeljivanje poklon bona korisniku
                await databaseContext.CustomersRewards.AddAsync(
                    new CustomerReward
                    {
                        AgentId = agentId,
                        CustomerId = customer!.Id,
                        CampaignId = campaignId,
                        RewardAmount = rewardAmount,
                        RewardDate = DateTime.Now
                    }
                );

                await databaseContext.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw new Exception("Reward for customer is not added.");
            }
        }
        else
            throw new Exception("Active campaing doesnt exist.");
    }

    //provjera da li adresa postoji u bazi
    public async Task<bool> AddressExists(Dtos.Address address)
    {
        return await databaseContext.Address.AnyAsync(
            a =>
                a.Street == address.Street
                && a.City == address.City
                && a.State == address.State
                && a.Zip == address.Zip
        );
    }

    //dobijanje adrese iz baze
    public async Task<Address> GetAdressFromDb(Dtos.Address address)
    {
        return await databaseContext.Address
            .Where(
                a =>
                    a.Street == address.Street
                    && a.City == address.City
                    && a.State == address.State
                    && a.Zip == address.Zip
            )
            .FirstOrDefaultAsync();
    }
}
