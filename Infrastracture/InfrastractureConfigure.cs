using Auth.Services;
using CustomerRewards.Auth.Entities;
using CustomerRewards.Auth.Services;
using CustomerRewards.Auth.TokenClaimGenerator;
using CustomerRewards.Infrastructure;
using Infrastracture.Mapper;
using Infrastracture.Services.CsvReportJob;
using Infrastracture.Services.CustomerRewardService;
using Infrastracture.Services.ExternalCustomerService;
using Infrastracture.Services.UsedRewardService;
using Infrastracture.TokenClaimGenerator;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

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
            services.AddScoped<IClaimInjectService, ClaimInjectService>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<TokenReaderService>();

            services
                .AddIdentity<User, Role>(options =>
                {
                    options.User.RequireUniqueEmail = true;
                    options.Password.RequireNonAlphanumeric = false;
                })
                .AddEntityFrameworkStores<DatabaseContext>()
                .AddDefaultTokenProviders();

            #endregion

            #region
            services.AddAutoMapper(typeof(MappingProfile));
            #endregion

            #region Services
            services.AddScoped<ExternalCustomerService>();
            services.AddScoped<CustomerRewardService>();
            services.AddScoped<UsedRewardService>();
            services.AddScoped<GenerateCsvFileService>();
            #endregion
        }
    }
}
