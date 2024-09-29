using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;

namespace Auth.Services
{
    public static class CustomClaimTypes
    {
        public const string AgentId = "agentId";
        public const string ActiveCampaign = "activeCampaign";
    }

    public class TokenReaderService
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public TokenReaderService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        private string GetClaimValue(string claimType)
        {
            var token = httpContextAccessor.HttpContext
                ?.Request.Headers["Authorization"].ToString()
                ?.Replace("Bearer ", "");

            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentException("No token provided");
            }

            var handler = new JwtSecurityTokenHandler();

            if (handler.CanReadToken(token))
            {
                var jwtToken = handler.ReadJwtToken(token);
                var claim = jwtToken.Claims.FirstOrDefault(c => c.Type == claimType);
                return claim?.Value;
            }
            return "";
        }

        public int GetAgentId()
        {
            return int.Parse(GetClaimValue(CustomClaimTypes.AgentId));
        }

        public int GetCampaignId()
        {
            return int.Parse(GetClaimValue(CustomClaimTypes.ActiveCampaign));
        }
    }
}
