using Microsoft.AspNetCore.Identity;

namespace CustomerRewards.Auth.Entities;

public class UserLogin : IdentityUserLogin<Guid>
{
    public override string LoginProvider { get; set; }
    public override string ProviderKey { get; set; }
    public override string ProviderDisplayName { get; set; }
    public override Guid UserId { get; set; }
    public virtual User User { get; set; }
}
