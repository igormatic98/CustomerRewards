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

    /// <summary>
    ///Dodjeljivanje popusta vijernim kupcima
    /// </summary>
    /// <param name="customerId">Id kupca</param>
    /// <param name="rewardAmount">Iznos bona koji dobija kupac</param>
    /// <returns></returns>
    [HttpPost("{customerId}/{rewardAmount}")]
    [Authorize(Roles = Role.AGENT)]
    public async Task<IActionResult> CreateRewardForCustomer(int customerId, decimal rewardAmount)
    {
        await customerRewardService.GetExternalCustomer(customerId, rewardAmount);
        return Ok();
    }
}
