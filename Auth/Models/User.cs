using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace CustomerRewards.Auth.Entities
{
    public class User : IdentityUser<Guid>
    {
        //public override Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public override string UserName { get; set; }

        [Required]
        [MaxLength(100)]
        public override string Email { get; set; }

        [MaxLength(20)]
        public override string PhoneNumber { get; set; }

        #region Properties from IdentityUser for [JsonIgnore]
        [JsonIgnore]
        public override string NormalizedUserName { get; set; }

        [JsonIgnore]
        public override string NormalizedEmail { get; set; }

        [JsonIgnore]
        public override string PasswordHash { get; set; }

        [JsonIgnore]
        public override string SecurityStamp { get; set; }

        [JsonIgnore]
        public override string ConcurrencyStamp { get; set; }

        [JsonIgnore]
        public override bool EmailConfirmed { get; set; }

        [JsonIgnore]
        public override bool PhoneNumberConfirmed { get; set; }

        [JsonIgnore]
        public override bool TwoFactorEnabled { get; set; }

        [JsonIgnore]
        public override bool LockoutEnabled { get; set; }

        [JsonIgnore]
        public override DateTimeOffset? LockoutEnd { get; set; }

        [JsonIgnore]
        public override int AccessFailedCount { get; set; }
        #endregion

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [JsonIgnore]
        [MinLength(8)]
        public string TemporaryPassword { get; set; }

        public string Picture { get; set; }

        [MaxLength(100)]
        public string AlternativeEmail { get; set; }

        public bool Active { get; set; }

        public virtual ICollection<UserRole> Roles { get; set; }

        public virtual ICollection<UserToken> UserTokens { get; set; }

        public virtual ICollection<UserLogin> Logins { get; set; }

        public virtual ICollection<UserClaim> Claims { get; set; }

        [MaxLength(13)]
        public string UniqueIdentifier { get; set; }
    }
}
