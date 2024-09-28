namespace CustomerRewards.Auth.Dtos;

public class RoleResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string NormalizedName { get; set; }
    public string Description { get; set; }
    public virtual ICollection<RoleClaimsDto> Claims { get; set; }
}
