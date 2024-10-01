using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerRewards.Company.Entities;

/// <summary>
/// Veza izmedju agenta i kampanje
/// Agent moze da radi na vise kampanja, kampanju vodi vise agenata
/// </summary>
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
