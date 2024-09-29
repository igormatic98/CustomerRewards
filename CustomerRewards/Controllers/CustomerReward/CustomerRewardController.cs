using CustomerRewards.Auth.Entities;
using Infrastracture.Services.CustomerRewardService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CustomerRewards.Controllers.CustomerReward;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("api/[controller]")]
[ApiController]
public class CustomerRewardController : ControllerBase
{
    private readonly CustomerRewardService customerRewardService;

    public CustomerRewardController(CustomerRewardService customerRewardService)
    {
        this.customerRewardService = customerRewardService;
    }

    [HttpPost]
    [Authorize(Roles = Role.AGENT)]
    public async Task<IActionResult> CreateRewardForCustomer()
    {
        await customerRewardService.GetExternalCustomer(1);
        return Ok();
    }
}
