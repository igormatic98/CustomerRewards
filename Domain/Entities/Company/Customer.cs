using CustomerRewards.Catalog.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomerRewards.Company.Entities;

[PrimaryKey(nameof(Id))]
[Microsoft.EntityFrameworkCore.Index(nameof(ExternalId), IsUnique = true)]
public class Customer
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int ExternalId { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string Ssn { get; set; }

    [Required]
    public DateTime Dob { get; set; }

    [Required]
    public int Age { get; set; }

    public int HomeId { get; set; }
    public int OfficeId { get; set; }

    [ForeignKey(nameof(HomeId))]
    public Address Home { get; set; }

    [ForeignKey(nameof(OfficeId))]
    public Address Office { get; set; }
}
