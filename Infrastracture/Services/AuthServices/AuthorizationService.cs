using System;
using CustomerRewards.Auth;
using CustomerRewards.Auth.Entities;
using CustomerRewards.Infrastructure;
using Microsoft.AspNetCore.Identity;

namespace CustomerRewards.Auth.Services;

public class AuthorizationService : IAuthorizationService
{
    private readonly DatabaseContext userContext;
    private readonly UserManager<User> userManager;
    private readonly RoleManager<Role> roleManager;

    public AuthorizationService(
        DatabaseContext userContext,
        UserManager<User> userManager,
        RoleManager<Role> roleManager
    )
    {
        this.userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
        this.userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        this.roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
    }
}
