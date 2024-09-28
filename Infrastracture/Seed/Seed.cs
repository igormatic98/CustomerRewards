using CustomerRewards.Auth.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastracture.Seed;

public static class Seed
{
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

        // Kreirajte defaultnog korisnika ako ne postoji
        var defaultUser = await userManager.FindByEmailAsync("agent@gmail.com");
        if (defaultUser == null)
        {
            var newUser = new User
            {
                UserName = "agent@gmail.com",
                FirstName = "Igor",
                LastName = "Matic",
                Email = "agent@gmail.com",
                EmailConfirmed = true,
                PhoneNumber = "",
                Active = true
            };

            var result = await userManager.CreateAsync(newUser, "Agent123");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(newUser, Role.AGENT);
            }
        }
    }
}
