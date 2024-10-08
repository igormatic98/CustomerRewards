﻿using Auth.Services;
using CustomerRewards.Auth.Entities;
using CustomerRewards.Auth.TokenClaimGenerator;
using CustomerRewards.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Infrastracture.TokenClaimGenerator;

/// <summary>
/// Servis za kreiranje dodatnih claim-ova u zavisnosti od role
/// Poziva se prilikom prijave korisnika i generisanja tokena
/// </summary>
public class ClaimInjectService : IClaimInjectService
{
    private readonly DatabaseContext databaseContext;
    private readonly UserManager<User> userManager;

    public ClaimInjectService(DatabaseContext databaseContext, UserManager<User> userManager)
    {
        this.databaseContext = databaseContext;
        this.userManager = userManager;
    }

    public async Task<List<Claim>> InjectClaimsForToken(User user)
    {
        var currentDate = DateTime.Now;
        var claims = new List<Claim>();
        var roles = await userManager.GetRolesAsync(user);

        //Ukoliko je korisnik u roli Agenta, u tokenu ce mu biti i AgentId, kao i aktivna kampanja za koju je zaduzen
        if (roles.Any(r => r == Role.AGENT))
        {
            var activeCampaign = await databaseContext.AgentCampaigns
                .Where(
                    ac =>
                        ac.Agent.UserId == user.Id
                        && ac.Campaign.StartDate <= currentDate
                        && ac.Campaign.EndDate >= currentDate
                )
                .FirstOrDefaultAsync();
            if (activeCampaign != null)
            {
                claims.Add(
                    new Claim(
                        CustomClaimTypes.AgentId,
                        activeCampaign.AgentId.ToString(),
                        ClaimValueTypes.Integer32
                    )
                );
                claims.Add(
                    new Claim(
                        CustomClaimTypes.ActiveCampaign,
                        activeCampaign.CampaignId.ToString(),
                        ClaimValueTypes.Integer32
                    )
                );
            }
        }
        //Ukoliko je korisnik u roli prodavca, u svom tokenu ima informaciju o aktivnoj kampanji
        if (roles.Any(r => r == Role.SELLER))
        {
            var activeCampaignId = await databaseContext.Campaigns
                .Where(c => c.StartDate <= currentDate && c.EndDate >= currentDate)
                .Select(c => c.Id)
                .FirstOrDefaultAsync();
            if (activeCampaignId != 0)
            {
                claims.Add(
                    new Claim(
                        CustomClaimTypes.ActiveCampaign,
                        activeCampaignId.ToString()!,
                        ClaimValueTypes.Integer32
                    )
                );
            }
        }
        return claims;
    }
}
