using CustomerRewards.Infrastructure;
using Infrastracture.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Infrastracture.Services.CsvReportJob;

public class CsvReportJob
{
    private readonly DatabaseContext databaseContext;
    public readonly GenerateCsvFileService generateCsvFileService;

    public CsvReportJob(
        DatabaseContext databaseContext,
        GenerateCsvFileService generateCsvFileService
    )
    {
        this.databaseContext = databaseContext;
        this.generateCsvFileService = generateCsvFileService;
    }

    public async Task FindCustomersWithSuccessfulBuy()
    {
        var currentDate = DateTime.Now;

        //id kampanje koja se zavrsila prije mjesec dana, ako postoji takva
        var campaign = await databaseContext.Campaigns
            .Where(c => c.EndDate.AddMonths(1).Date == currentDate.Date)
            .Select(c => new { Id = c.Id, Name = c.Name })
            .FirstOrDefaultAsync();

        //ako postoji kampanja zavrsena prije mjesec dana
        if (campaign! != null)
        {
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

            await generateCsvFileService.CreateCsv(customers, campaign.Name);
        }
    }
}
