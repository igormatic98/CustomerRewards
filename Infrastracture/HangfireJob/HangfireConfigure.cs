using Hangfire;
using Hangfire.SqlServer;
using Infrastracture.Services.CsvReportJob;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastracture.HangfireJob;

/// <summary>
/// Statička klasa koja služi za registrovanje Hangfire jobova i definisanje metoda koje jobovi izvršavaju
/// </summary>
public static class HangfireConfigure
{
    public static void Register(IServiceCollection services, IConfiguration configuration)
    {
        services.AddHangfire(
            config =>
                config
                    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UseSqlServerStorage(configuration["ConnectionString:SQL"])
        );

        services.AddHangfireServer();
        services.AddScoped<CsvReportJob>();
    }

    public static void InitializeJobs(
        this IApplicationBuilder app,
        WebApplicationBuilder applicationBuilder
    )
    {
        app.UseHangfireDashboard();

        RecurringJob.AddOrUpdate<CsvReportJob>(
            "Reports",
            cj => cj.FindCustomersWithSuccessfulBuy(),
            Cron.Daily(12, 00),
            new RecurringJobOptions() { TimeZone = TimeZoneInfo.Local }
        );
    }
}
