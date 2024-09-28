using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CustomerRewards.Auth.Dtos;

public class UserRegister
{
    public string UserName { get; set; }

    public string Email { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string PhoneNumber { get; set; }

    public string AlternativeEmail { get; set; }

    public bool? Active { get; set; }

    public string Password { get; set; }

    public List<string> RoleNames { get; set; }

    public string Picture { get; set; }

    public string UniqueIdentifier { get; set; }

    public string PermanentResidence { get; set; }
    public string Workplace { get; set; }
}
