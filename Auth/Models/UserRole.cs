using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CustomerRewards.Auth.Entities;

[PrimaryKey(nameof(UserId), nameof(RoleId))]
public class UserRole : IdentityUserRole<Guid>
{
    public override Guid UserId { get; set; }
    public override Guid RoleId { get; set; }
    public User User { get; set; }
    public Role Role { get; set; }
}
