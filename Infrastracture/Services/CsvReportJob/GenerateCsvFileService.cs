using CsvHelper.Configuration;
using CsvHelper;
using System.Globalization;
using Microsoft.Extensions.Configuration;

namespace Infrastracture.Services.CsvReportJob;

public class GenerateCsvFileService
{
    private readonly IConfiguration configuration;
    private readonly string currentPath = Directory.GetCurrentDirectory();

    public GenerateCsvFileService(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public async Task CreateCsv<T>(List<T> data, string fileName)
    {
        var folderPath = Path.Combine(currentPath, configuration["CsvSettings:csvPath"]!);
        if (!Directory.Exists(folderPath))

            Directory.CreateDirectory(folderPath);

        var filePath = Path.Combine(folderPath, $"{fileName}.csv");

        if (!File.Exists(filePath))
            using (var fileStream = File.Create(filePath)) { }

        using (var writer = new StreamWriter(filePath))
        using (
            var csv = new CsvWriter(
                writer,
                new CsvConfiguration(CultureInfo.InvariantCulture) { Delimiter = ",", }
            )
        )
        {
            await csv.WriteRecordsAsync(data);
        }
    }
}
