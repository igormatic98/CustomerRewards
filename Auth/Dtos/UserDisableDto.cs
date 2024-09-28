using System;

namespace CustomerRewards.Auth.Dtos;

public class UserAccountControlDto
{
    public Guid UserId { get; set; }
    public string ProviderUserId { get; set; }
}
