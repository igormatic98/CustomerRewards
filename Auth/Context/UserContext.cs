using CustomerRewards.Auth.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CustomerRewards.Auth
{
    public class UserContext
        : IdentityDbContext<User, Role, Guid, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
    {
        public UserContext(DbContextOptions<UserContext> dbContextOptions)
            : base(dbContextOptions)
        {
            this.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema("Auth");

            modelBuilder.Entity<UserRole>(userRole =>
            {
                userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

                userRole
                    .HasOne(ur => ur.Role)
                    .WithMany(r => r.Users)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                userRole
                    .HasOne(ur => ur.User)
                    .WithMany(r => r.Roles)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();

                userRole.ToTable("UserRole");
            });

            modelBuilder.Entity<User>(c =>
            {
                c.ToTable("User", "Auth");
            });

            modelBuilder.Entity<Role>(c =>
            {
                c.ToTable("Role", "Auth");
            });

            modelBuilder.Entity<RoleClaim>(c =>
            {
                c.HasOne(roleClaim => roleClaim.Role)
                    .WithMany(role => role.Claims)
                    .HasForeignKey(roleClaim => roleClaim.RoleId);
                c.ToTable("RoleClaim", "Auth");
            });

            modelBuilder.Entity<UserClaim>(c =>
            {
                c.HasOne(userClaim => userClaim.User)
                    .WithMany(user => user.Claims)
                    .HasForeignKey(userClaim => userClaim.UserId);
                c.ToTable("UserClaim", "Auth");
            });

            modelBuilder.Entity<UserLogin>(c =>
            {
                c.HasOne(userLogin => userLogin.User)
                    .WithMany(user => user.Logins)
                    .HasForeignKey(userLogin => userLogin.UserId);
                c.ToTable("UserLogin", "Auth");
            });

            modelBuilder.Entity<UserRole>(c =>
            {
                c.HasOne(userRole => userRole.Role)
                    .WithMany(role => role.Users)
                    .HasForeignKey(userRole => userRole.RoleId);
                c.HasOne(userRole => userRole.User)
                    .WithMany(user => user.Roles)
                    .HasForeignKey(userRole => userRole.UserId);
                c.ToTable("UserRole", "Auth");
            });

            modelBuilder.Entity<UserToken>(c =>
            {
                c.HasOne(userToken => userToken.User)
                    .WithMany(user => user.UserTokens)
                    .HasForeignKey(userToken => userToken.UserId);
                c.ToTable("UserToken", "Auth");
            });
        }
    }
}
