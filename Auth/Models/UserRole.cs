using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CustomerRewards.Auth.Entities;

[PrimaryKey(nameof(UserId), nameof(RoleId))]
public class UserRole : IdentityUserRole<Guid>
{
    public override Guid UserId { get; set; }
    public override Guid RoleId { get; set; }
    public virtual User User { get; set; }
    public virtual Role Role { get; set; }
}
