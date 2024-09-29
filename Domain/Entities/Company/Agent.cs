using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CustomerRewards.Auth.Entities;
using Microsoft.EntityFrameworkCore;
using Domain.Entities.Catalog;

namespace Domain.Entities.Company;

[PrimaryKey(nameof(Id))]
public class Agent
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int UserId { get; set; }

    [Required]
    public string ContractNumber { get; set; }
    public int? AddressId { get; set; }

    [ForeignKey(nameof(AddressId))]
    public virtual Address Address { get; set; }

    [ForeignKey("UserId")]
    public virtual User User { get; set; }
}
