using Microsoft.AspNetCore.Identity;

namespace CustomerRewards.Auth.Entities;

public class Role : IdentityRole<Guid>
{
    public const string AGENT = "Agent";
    public const string CUSTOMER = "Customer";
    public const string DIRECTOR = "Director";
    public const string SELLER = "Seller";

    public Role()
        : base() { }

    public Role(string roleName, string description)
        : this()
    {
        Name = roleName;
        Description = description;
    }

    public ICollection<UserRole> Users { get; set; }
    public ICollection<RoleClaim> Claims { get; set; }
    public string Description { get; set; }
}
