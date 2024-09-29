using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Company;

[PrimaryKey(nameof(AgentId), nameof(CampaignId))]
public class AgentCampaign
{
    public int AgentId { get; set; }

    [ForeignKey("AgentId")]
    public virtual Agent Agent { get; set; }

    public int CampaignId { get; set; }

    [ForeignKey("CampaignId")]
    public virtual Campaign Campaign { get; set; }
}
