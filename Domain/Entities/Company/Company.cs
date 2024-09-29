using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerRewards.Company.Entities;

[PrimaryKey(nameof(Id))]
public class Company
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string ContactInfo { get; set; }

    public string Code { get; set; }

    public virtual ICollection<Campaign> Campaigns { get; set; }
}
