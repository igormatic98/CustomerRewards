using Microsoft.AspNetCore.Identity;

namespace CustomerRewards.Auth.Entities;

public class Role : IdentityRole<Guid>
{
    public const string AGENT = "Agent";
    public const string CUSTOMER = "Customer";
    public const string DIRECTOR = "Director";

    public Role()
        : base() { }

    public Role(string roleName, string description)
        : this()
    {
        Name = roleName;
        Description = description;
    }

    public virtual ICollection<UserRole> Users { get; set; }
    public virtual ICollection<RoleClaim> Claims { get; set; }
    public string Description { get; set; }
}
