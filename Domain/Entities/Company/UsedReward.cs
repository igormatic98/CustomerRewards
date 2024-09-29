using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Company;

[PrimaryKey(nameof(Id))]
public class UsedReward
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public int CampaignId { get; set; }

    [ForeignKey(nameof(CustomerId) + "," + nameof(CampaignId))]
    public CustomerReward CustomerReward { get; set; }
    public DateTime UsedDate { get; set; }
    public decimal UsedAmount { get; set; }
}
