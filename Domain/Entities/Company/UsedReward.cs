using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerRewards.Company.Entities;

/// <summary>
/// Iskoristeni bonovi
/// Informacije o tome kada je iskoristen, koliki je iznos bona
/// </summary>
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
