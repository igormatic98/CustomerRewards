using CustomerRewards.Auth.Entities;
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

    [HttpPost]
    [Authorize(Roles = Role.DIRECTOR)]
    public async Task<IActionResult> CreateCompany()
    {
        return Ok();
    }
}
