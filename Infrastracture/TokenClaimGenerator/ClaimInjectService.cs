using Auth.Services;
using CustomerRewards.Auth.Entities;
using CustomerRewards.Auth.TokenClaimGenerator;
using CustomerRewards.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Infrastracture.TokenClaimGenerator;

public class ClaimInjectService : IClaimInjectService
{
    private readonly DatabaseContext databaseContext;
    private readonly UserManager<User> userManager;

    public ClaimInjectService(DatabaseContext databaseContext, UserManager<User> userManager)
    {
        this.databaseContext = databaseContext;
        this.userManager = userManager;
    }

    public async Task<List<Claim>> InjectClaimsForToken(User user, string oldAccessToken)
    {
        var currentDate = DateTime.Now;
        var claims = new List<Claim>();
        var roles = await userManager.GetRolesAsync(user);

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
        return claims;
    }
}
