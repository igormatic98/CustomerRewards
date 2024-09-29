using CustomerRewards.Auth.Entities;
using CustomerRewards.Auth.Services;
using CustomerRewards.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Infrastracture
{
    public class InfrastractureConfigure
    {
        public static void Register(
            IServiceCollection services,
            IConfiguration configuration,
            IWebHostEnvironment environment
        )
        {
            services.AddDbContext<DatabaseContext>(opts =>
            {
                opts.UseSqlServer(configuration["ConnectionString:SQL"]);

                if (environment.IsDevelopment())
                {
                    opts.EnableSensitiveDataLogging();
                    opts.LogTo(x => Debug.WriteLine(x), LogLevel.Debug);
                }
            });
            #region Auth
            services.AddScoped<IAuthenticationService, AuthenticationService>();

            services
                .AddIdentity<User, Role>(options =>
                {
                    options.User.RequireUniqueEmail = true;
                    options.Password.RequireNonAlphanumeric = false;
                })
                .AddEntityFrameworkStores<DatabaseContext>()
                .AddDefaultTokenProviders();

            #endregion
        }
    }
}
