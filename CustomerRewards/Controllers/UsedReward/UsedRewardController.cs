﻿using CustomerRewards.Auth.Entities;
using Infrastracture.Services.UsedRewardService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CustomerRewards.Controllers.UsedReward;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("api/[controller]")]
[ApiController]
public class UsedRewardController : ControllerBase
{
    private readonly UsedRewardService usedRewardService;

    public UsedRewardController(UsedRewardService usedRewardService)
    {
        this.usedRewardService = usedRewardService;
    }

    /// <summary>
    /// Potvrda o uspjesnoj kupovini i koristenju bona od strane nagradjenog kupca
    /// </summary>
    /// <param name="customerId">Id kupca</param>
    /// <param name="usedAmount">Iznos koji je iskoristio od ukupne vrijednosti bona</param>
    /// <returns></returns>
    [HttpPost("{customerId}/{usedAmount}")]
    [Authorize(Roles = Role.SELLER)]
    public async Task<IActionResult> UsedRewardByCustomer(int customerId, decimal usedAmount)
    {
        await usedRewardService.CustomerUsedReward(customerId, usedAmount);
        return Ok();
    }
}
