using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CustomerRewards.Auth.Entities;
using Microsoft.EntityFrameworkCore;
using CustomerRewards.Catalog.Entities;

namespace CustomerRewards.Company.Entities;

/// <summary>
/// Agent koji je prosirena versija Usera, kao zaposleni ima poslovni kontakt i adresu
/// </summary>
[PrimaryKey(nameof(Id))]
public class Agent
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public Guid UserId { get; set; }

    [Required]
    public string ContractNumber { get; set; }
    public int? AddressId { get; set; }

    [ForeignKey(nameof(AddressId))]
    public Address Address { get; set; }

    [ForeignKey(nameof(UserId))]
    public User User { get; set; }

    public ICollection<AgentCampaign> AgentCampaigns { get; set; }
}
