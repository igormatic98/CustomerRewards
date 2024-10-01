using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CustomerRewards.Auth.Entities;

[PrimaryKey(nameof(Id))]
public class RoleClaim : IdentityRoleClaim<Guid>
{
    public override int Id { get; set; }
    public override Guid RoleId { get; set; }
    public override string ClaimType { get; set; }
    public override string ClaimValue { get; set; }
    public Role Role { get; set; }
}
