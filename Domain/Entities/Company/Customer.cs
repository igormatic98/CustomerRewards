using Domain.Entities.Catalog;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Company;

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
    public string Dob { get; set; }

    [Required]
    public int Age { get; set; }

    public int HomeId { get; set; }
    public int OfficeId { get; set; }

    [ForeignKey(nameof(HomeId))]
    public virtual Address Home { get; set; }

    [ForeignKey(nameof(OfficeId))]
    public virtual Address Office { get; set; }
}
