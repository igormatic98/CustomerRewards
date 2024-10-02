using CustomerRewards.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CustomerRewards.Controllers.CampaignController;

/// <summary>
/// Kontroler za generisane dokumente kampanje
/// </summary>
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("api/[controller]")]
[ApiController]
public class CampaignDocumentController : ControllerBase
{
    private readonly DatabaseContext databaseContext;
    private readonly string currentPath = Directory.GetCurrentDirectory();

    public CampaignDocumentController(DatabaseContext databaseContext)
    {
        this.databaseContext = databaseContext;
    }

    /// <summary>
    /// Zahtjev za dobijanje fajla sa podacima o iskoristenim bonovima za kupovinu
    /// </summary>
    /// <param name="campaignId">Id kampanje</param>
    /// <returns></returns>
    [HttpGet("{campaignId}")]
    [Authorize(Roles = "Director,Agent")]
    public async Task<IActionResult> GetCsvDocumentForCampaign(int campaignId)
    {
        //pronalazimo fajl za datu kampanju i putanju do njega
        var path = await databaseContext.CampaignDocuments
            .Where(cd => cd.CampaignId == campaignId)
            .Select(cd => cd.Path)
            .FirstOrDefaultAsync();

        //ukoliko nema datog fajla obavjestavamo o tome
        if (path == null)
            return NotFound("File is not generated.");

        var fullPath = Path.Combine(currentPath, path);
        if (!System.IO.File.Exists(fullPath))
            return NotFound("File not found on the server.");

        var bytes = System.IO.File.ReadAllBytes(fullPath);
        return File(bytes, "application/octet-stream", Path.GetFileName(fullPath));
    }
}
