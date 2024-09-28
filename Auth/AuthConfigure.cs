using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CustomerRewards.Auth;

using CustomerRewards.Auth.Entities;
using CustomerRewards.Auth.Services;
using Microsoft.AspNetCore.Hosting;

public static class AuthConfigure
{
    public static void Register(
        IServiceCollection services,
        IConfiguration configuration,
        IWebHostEnvironment environment
    )
    {
        services.AddDbContext<UserContext>(opts =>
        {
            opts.UseSqlServer(configuration["ConnectionString:SQL"]);

            if (environment.IsDevelopment())
            {
                opts.EnableSensitiveDataLogging();
                opts.LogTo(x => Debug.WriteLine(x), LogLevel.Debug);
            }
        });

        services.AddScoped<IAuthenticationService, AuthenticationService>();

        services
            .AddIdentity<User, Role>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequireNonAlphanumeric = false;
            })
            .AddEntityFrameworkStores<UserContext>()
            .AddDefaultTokenProviders();
    }
}
