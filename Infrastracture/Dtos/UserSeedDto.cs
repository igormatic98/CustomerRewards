using CustomerRewards.Auth.Entities;
using CustomerRewards.Infrastructure;
using Microsoft.AspNetCore.Identity;

namespace Infrastracture.Dtos;

/// <summary>
/// Dto koji se koristi u Seed-u prilikom pokretanja programa
/// za kreiranje pocetnih vrijednosti u bazi
/// </summary>
public class UserSeedDto
{
    public User User { get; set; }
    public string RoleName { get; set; }
    public UserManager<User> UserManager { get; set; }
    public DatabaseContext DatabaseContext { get; set; }

    public UserSeedDto(
        User user,
        string roleName,
        UserManager<User> userManager,
        DatabaseContext databaseContext
    )
    {
        User = user;
        RoleName = roleName;
        UserManager = userManager;
        DatabaseContext = databaseContext;
    }
}
