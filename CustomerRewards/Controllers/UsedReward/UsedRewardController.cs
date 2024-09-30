using CustomerRewards.Auth.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CustomerRewards.Controllers.UsedReward;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("api/[controller]")]
[ApiController]
public class UsedRewardController : ControllerBase
{
    public UsedRewardController() { }

    [HttpPost("{customerId}/{rewardAmount}")]
    [Authorize(Roles = Role.SELLER)]
    public async Task<IActionResult> UsedRewardByCustomer()
    {
        return Ok();
    }
}
