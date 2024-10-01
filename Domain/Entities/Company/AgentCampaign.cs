using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerRewards.Company.Entities;

[PrimaryKey(nameof(AgentId), nameof(CampaignId))]
public class AgentCampaign
{
    public int AgentId { get; set; }

    [ForeignKey(nameof(AgentId))]
    public Agent Agent { get; set; }

    public int CampaignId { get; set; }

    [ForeignKey(nameof(CampaignId))]
    public Campaign Campaign { get; set; }
}
