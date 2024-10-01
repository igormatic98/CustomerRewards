using CustomerRewards.Infrastructure;

namespace Infrastracture.Services.CustomerRewardService;

using Auth.Services;
using AutoMapper;
using CustomerRewards.Catalog.Entities;
using CustomerRewards.Company.Entities;
using Infrastracture.Services.ExternalCustomerService;
using Microsoft.EntityFrameworkCore;

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
        var currentDay = DateTime.Now.Day;
        var numberOfAddedCustomerByAgent = await databaseContext.CustomersRewards
            .Where(
                cr =>
                    cr.AgentId == agentId
                    && cr.CampaignId == campaignId
                    && cr.RewardDate.Day == currentDay
            )
            .CountAsync();
        if (numberOfAddedCustomerByAgent > 5)
            throw new Exception(
                "Agent added already max(5) customer to reward on this campaign today."
            );

        var isRewardAlradyGiven = await databaseContext.CustomersRewards.AnyAsync(
            cr => cr.Customer.ExternalId == customerId && cr.CampaignId == campaignId
        );
        if (isRewardAlradyGiven)
            throw new Exception("Reward is already given to this customer.");

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
                    var findPersonResult = await externalCustomerService.GetExternalCustomer(
                        customerId
                    );

                    if (findPersonResult != null)
                    {
                        bool homeExists = await AddressExists(findPersonResult.Home);
                        if (!homeExists)
                        {
                            home = mapper.Map<Address>(findPersonResult.Home);
                            await databaseContext.Address.AddAsync(home);
                        }
                        else
                            home = await GetAdressFromDb(findPersonResult.Home);

                        // Proveri da li postoji office adresa
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

                    customer = await databaseContext.Customers
                        .Where(c => c.ExternalId == customerId)
                        .FirstOrDefaultAsync();

                await databaseContext.CustomersRewards.AddAsync(
                    new CustomerReward
                    {
                        AgentId = agentId,
                        CustomerId = customer.Id,
                        CampaignId = campaignId,
                        RewardAmount = rewardAmount,
                        RewardDate = DateTime.Now
                    }
                );

                await databaseContext.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception("Reward for customer is not added.");
            }
        }
        else
            throw new Exception("Active campaing doesnt exist.");
    }

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
