using CustomerRewards.Infrastructure;

namespace Infrastracture.Services.CustomerRewardService;

using Auth.Services;
using AutoMapper;
using CustomerRewards.Catalog.Entities;
using Infrastracture.Services.ExternalCustomerService;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data.Entity;

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

    public async Task GetExternalCustomer(int customerId)
    {
        var agentId = tokenReaderService.GetAgentId();
        var campaignId = tokenReaderService.GetCampaignId();

        if (campaignId != 0)
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
                        var home = mapper.Map<Address>(findPersonResult.Home);
                        await databaseContext.Address.AddAsync(home);
                    }

                    // Proveri da li postoji office adresa
                    bool officeExists = await AddressExists(findPersonResult.Office);
                    if (!officeExists)
                    {
                        var office = mapper.Map<Address>(findPersonResult.Home);
                        await databaseContext.Address.AddAsync(office);
                    }
                    await databaseContext.SaveChangesAsync();
                }
            }

            // Sačuvaj promene u bazi podataka
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
}
