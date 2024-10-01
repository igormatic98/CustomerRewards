using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerRewards.Company.Entities;

/// <summary>
/// Pocetna tabela, informacije o samoj kompaniji
/// Ima vise kampanja
/// </summary>
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

    public ICollection<Campaign> Campaigns { get; set; }
}
