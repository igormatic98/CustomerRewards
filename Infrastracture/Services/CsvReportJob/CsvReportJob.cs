using CustomerRewards.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
}
