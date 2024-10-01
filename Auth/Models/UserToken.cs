using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CustomerRewards.Auth.Entities;

[PrimaryKey(nameof(UserId), nameof(LoginProvider), nameof(Name))]
public class UserToken : IdentityUserToken<Guid>
{
    public override Guid UserId { get; set; }

    public override string LoginProvider { get; set; }

    public override string Name { get; set; }

    [ProtectedPersonalData]
    public override string Value { get; set; }
    public User User { get; set; }
}
