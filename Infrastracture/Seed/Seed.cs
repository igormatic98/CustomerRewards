using CustomerRewards.Auth.Entities;
using CustomerRewards.Company.Entities;
using CustomerRewards.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastracture.Seed;

public static class Seed
{
    public const string Password = "Test123";

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

        string[] roles = { Role.DIRECTOR, Role.AGENT, Role.CUSTOMER };
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
            }
        }
        //kreiranje direktora
        await CreateUser(
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
        );

        //kreiranje agenta
        await CreateUser(
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
        );
    }

    public static async Task CreateUser(
        User user,
        string roleName,
        UserManager<User> userManager,
        DatabaseContext databaseContext
    )
    {
        // Kreirajte defaultnog korisnika ako ne postoji
        var defaultUser = await userManager.FindByEmailAsync(user.Email);
        if (defaultUser == null)
        {
            var result = await userManager.CreateAsync(user, Password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, roleName);

                //Ovaj dio se moze zakomentarisati ukoliko se zeli testirati preko apija, na ovaj nacin sam skratio posao
                //nije toliki fokus na samoj kompaniji
                using (var transaction = await databaseContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        if (roleName == Role.AGENT)
                        {
                            var agent = new Agent { UserId = user.Id, ContractNumber = "24-T-12" };
                            await databaseContext.Agents.AddAsync(agent);
                            await databaseContext.SaveChangesAsync();

                            var company = new Company
                            {
                                Name = "Tesla XY",
                                Code = "21-AK1",
                                ContactInfo = "057/212-223",
                                Campaigns = new List<Campaign>
                                {
                                    new Campaign
                                    {
                                        Name = "Energija buducnosti ",
                                        StartDate = new DateTime(DateTime.Now.Year, 10, 1),
                                        EndDate = new DateTime(DateTime.Now.Year, 11, 15),
                                        AgentCampaigns = new List<AgentCampaign>
                                        {
                                            { new AgentCampaign { AgentId = agent.Id } }
                                        }
                                    }
                                },
                            };

                            await databaseContext.Companies.AddAsync(company);
                            await databaseContext.SaveChangesAsync();

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
