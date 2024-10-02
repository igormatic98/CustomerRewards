namespace CustomerRewards.Auth.Dtos;

public class RefreshTokenDTO
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public string IdToken { get; set; }
    public string SelectedRole { get; set; }
    public Dictionary<string, int?> GlobalFilters { get; set; }
}
