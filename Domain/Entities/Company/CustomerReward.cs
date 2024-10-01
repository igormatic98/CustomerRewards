using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerRewards.Company.Entities;

/// <summary>
/// Poklon bonovi koje unosi agent
/// Kompozinti kljuc kupac kampanja, da se ne bi desilo da isti kupac dobije dva bona za istu kampanju
/// Strani kljuc na agenta, da znamo ko je poklonio bon kupcu
/// </summary>
[PrimaryKey(nameof(CustomerId), nameof(CampaignId))]
public class CustomerReward
{
    public int CustomerId { get; set; }

    [ForeignKey(nameof(CustomerId))]
    public Customer Customer { get; set; }
    public int CampaignId { get; set; }

    [ForeignKey(nameof(CampaignId))]
    public Campaign Campaign { get; set; }
    public int AgentId { get; set; }

    [ForeignKey(nameof(AgentId))]
    public Agent Agent { get; set; }
    public DateTime RewardDate { get; set; }
    public decimal RewardAmount { get; set; }
}
