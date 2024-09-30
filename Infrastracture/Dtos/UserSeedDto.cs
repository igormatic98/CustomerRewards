using CustomerRewards.Auth.Entities;
using CustomerRewards.Infrastructure;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastracture.Dtos;

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
