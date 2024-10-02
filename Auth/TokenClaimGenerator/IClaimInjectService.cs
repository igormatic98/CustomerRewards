using CustomerRewards.Auth.Entities;
using System.Security.Claims;

namespace CustomerRewards.Auth.TokenClaimGenerator;

public interface IClaimInjectService
{
    Task<List<Claim>> InjectClaimsForToken(User user);
}
