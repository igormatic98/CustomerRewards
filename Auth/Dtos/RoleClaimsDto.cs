using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerRewards.Auth.Dtos;

public class RoleClaimsDto
{
    public string ClaimType { get; set; }
    public string ClaimValue { get; set; }
}
