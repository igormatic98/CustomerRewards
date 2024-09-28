using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CustomerRewards.Controllers.Company;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("api/[controller]")]
[ApiController]
public class CompanyController : ControllerBase
{
    public CompanyController() { }

    [HttpGet("add")]
    [Authorize(Roles = "Agent")]
    public async Task<IActionResult> GetCompanyDetails()
    {
        return Ok();
    }
}
