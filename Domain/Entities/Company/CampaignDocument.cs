using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerRewards.Company.Entities;

/// <summary>
/// Putanja do dokumenata generisanih mjesec dana nakon kampanje
/// Veza na kampanju na koju se odnosi dokument
/// </summary>
[PrimaryKey(nameof(Id))]
public class CampaignDocument
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int CampaignId { get; set; }

    [Required]
    public string Path { get; set; }
    public DateTime CreatedDate { get; set; }

    [ForeignKey(nameof(CampaignId))]
    public Campaign Campaign { get; set; }
}
