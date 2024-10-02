using CustomerRewards.Auth.Entities;
using CustomerRewards.Company.Entities;
using CustomerRewards.Infrastructure;
using Infrastracture.Dtos;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastracture.Seed;

/// <summary>
/// Pokrece se prilikom pokretanja aplikacije
/// Inicijalno pokretanje baze podataka sa korisnicima, rolama, kompanijom i agentom
/// </summary>
public static class Seed
{
    private const string Password = "Test123";

    public static async Task SeedAsync(
        IApplicationBuilder app,
        IConfiguration Configuration,
        IWebHostEnvironment environment
    )
    {
        using var scope = app.ApplicationServices.CreateScope();
        var services = scope.ServiceProvider;
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var databaseContext = services.GetRequiredService<DatabaseContext>();

        string[] roles = { Role.DIRECTOR, Role.AGENT, Role.CUSTOMER, Role.SELLER };
        //dodavanje rola
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                if (role == Role.DIRECTOR)
                    await roleManager.CreateAsync(
                        new Role
                        {
                            Name = role,
                            Description = "Direktor telekomunkacijske kompanije"
                        }
                    );

                if (role == Role.AGENT)
                    await roleManager.CreateAsync(
                        new Role
                        {
                            Name = role,
                            Description =
                                "Zaposleni koji vode kampanje unutar telekomunkacijske kompanije"
                        }
                    );

                if (role == Role.CUSTOMER)
                    await roleManager.CreateAsync(new Role { Name = role, Description = "Kupac" });

                if (role == Role.SELLER)
                    await roleManager.CreateAsync(
                        new Role { Name = role, Description = "Prodavac" }
                    );
            }
        }
        //dodavanje tri korisnika sa tri razlicite role dirketor, agent, saler
        await CreateUsers(
            new List<UserSeedDto>
            {
                new UserSeedDto(
                    new User
                    {
                        UserName = "director@gmail.com",
                        FirstName = "Petar",
                        LastName = "Markovic",
                        Email = "director@gmail.com",
                        EmailConfirmed = true,
                        PhoneNumber = "",
                        Active = true
                    },
                    Role.DIRECTOR,
                    userManager,
                    databaseContext
                ),
                new UserSeedDto(
                    new User
                    {
                        UserName = "agent@gmail.com",
                        FirstName = "Igor",
                        LastName = "Matic",
                        Email = "agent@gmail.com",
                        EmailConfirmed = true,
                        PhoneNumber = "",
                        Active = true
                    },
                    Role.AGENT,
                    userManager,
                    databaseContext
                ),
                new UserSeedDto(
                    new User
                    {
                        UserName = "seller@gmail.com",
                        FirstName = "Dzenan",
                        LastName = "Smajic",
                        Email = "seller@gmail.com",
                        EmailConfirmed = true,
                        PhoneNumber = "",
                        Active = true
                    },
                    Role.SELLER,
                    userManager,
                    databaseContext
                )
            }
        );
    }

    public static async Task CreateUsers(List<UserSeedDto> users)
    {
        foreach (var userDto in users)
        {
            // Kreirajte defaultnog korisnika ako ne postoji
            var defaultUser = await userDto.UserManager.FindByEmailAsync(userDto.User.Email);
            if (defaultUser == null)
            {
                var result = await userDto.UserManager.CreateAsync(userDto.User, Password);
                if (result.Succeeded)
                {
                    await userDto.UserManager.AddToRoleAsync(userDto.User, userDto.RoleName);
                    //Kreiranje Kompanije, kampanje i agenta, jer nije toliki fokus na apijima za njihovo kreiranje
                    using (
                        var transaction =
                            await userDto.DatabaseContext.Database.BeginTransactionAsync()
                    )
                    {
                        try
                        {
                            if (userDto.RoleName == Role.AGENT)
                            {
                                var agent = new Agent
                                {
                                    UserId = userDto.User.Id,
                                    ContractNumber = "24-T-12"
                                };
                                await userDto.DatabaseContext.Agents.AddAsync(agent);
                                await userDto.DatabaseContext.SaveChangesAsync();

                                var company = new Company
                                {
                                    Name = "Tesla XY",
                                    Code = "21-AK1",
                                    ContactInfo = "057/212-223",
                                    Campaigns = new List<Campaign>
                                    {
                                        new Campaign
                                        {
                                            Name = "Energija buducnosti",
                                            StartDate = new DateTime(DateTime.Now.Year, 10, 1),
                                            EndDate = new DateTime(DateTime.Now.Year, 11, 15),
                                            AgentCampaigns = new List<AgentCampaign>
                                            {
                                                { new AgentCampaign { AgentId = agent.Id } }
                                            }
                                        }
                                    },
                                };

                                await userDto.DatabaseContext.Companies.AddAsync(company);
                                await userDto.DatabaseContext.SaveChangesAsync();

                                await transaction.CommitAsync();
                            }
                        }
                        catch (Exception ex)
                        {
                            await transaction.RollbackAsync();
                        }
                    }
                }
            }
        }
    }
}
