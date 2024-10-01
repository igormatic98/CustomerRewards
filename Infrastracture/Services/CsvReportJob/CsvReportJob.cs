using CustomerRewards.Infrastructure;
using Infrastracture.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Infrastracture.Services.CsvReportJob;

using CustomerRewards.Company.Entities;
using Microsoft.Extensions.Configuration;

public class CsvReportJob
{
    private readonly DatabaseContext databaseContext;
    public readonly GenerateCsvFileService generateCsvFileService;
    private readonly IConfiguration configuration;
    private readonly string currentPath = Directory.GetCurrentDirectory();

    public CsvReportJob(
        DatabaseContext databaseContext,
        GenerateCsvFileService generateCsvFileService,
        IConfiguration configuration
    )
    {
        this.databaseContext = databaseContext;
        this.generateCsvFileService = generateCsvFileService;
        this.configuration = configuration;
    }

    public async Task FindCustomersWithSuccessfulBuy()
    {
        var currentDate = DateTime.Now;
        var fullPath = "";
        //id kampanje koja se zavrsila prije mjesec dana
        var campaign = await databaseContext.Campaigns
            .Where(c => c.EndDate.AddMonths(1).Date == currentDate.Date)
            .Select(c => new { Id = c.Id, Name = c.Name })
            .FirstOrDefaultAsync();

        //ako postoji kampanja
        if (campaign! != null)
        {
            var transaction = await databaseContext.Database.BeginTransactionAsync();
            try
            {
                //spisak kupaca koji su iskoristili nagradu za vrijeme date kampanje
                var customers = await databaseContext.UsedRewards
                    .Where(ur => ur.CampaignId == campaign.Id)
                    .Select(ur => ur.CustomerReward.Customer)
                    .Select(
                        c =>
                            new CustomerFileResultDto
                            {
                                Name = c.Name,
                                Age = c.Age,
                                Dob = c.Dob,
                                Ssn = c.Ssn,
                                Home = new AddressResultDto
                                {
                                    City = c.Home.City,
                                    State = c.Home.State,
                                    Street = c.Home.Street,
                                    Zip = c.Home.Zip
                                }
                            }
                    )
                    .ToListAsync();

                var folderPath = Path.Combine(currentPath, configuration["CsvSettings:csvPath"]!);

                fullPath = Path.Combine(folderPath, $"{campaign.Name}.csv");
                //generisanje csv fajla sa podacima o kupcima
                await generateCsvFileService.CreateCsv(customers, folderPath, campaign.Name);
                //upisivanje u bazu podataka putanju koja vodi do kreiranog fajla i veza sa kampanjom za koju je vezan
                await databaseContext.CampaignDocuments.AddAsync(
                    new CampaignDocument
                    {
                        CampaignId = campaign.Id,
                        CreatedDate = currentDate,
                        Path = $"{configuration["CsvSettings:csvPath"]}/{campaign.Name}.csv"
                    }
                );
                await databaseContext.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                if (System.IO.File.Exists(fullPath))
                    File.Delete(fullPath);

                throw new Exception("Csv file is not generated.");
            }
        }
    }
}
