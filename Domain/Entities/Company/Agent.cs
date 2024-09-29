﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CustomerRewards.Auth.Entities;
using Microsoft.EntityFrameworkCore;
using CustomerRewards.Catalog.Entities;

namespace CustomerRewards.Company.Entities;

[PrimaryKey(nameof(Id))]
public class Agent
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public Guid UserId { get; set; }

    [Required]
    public string ContractNumber { get; set; }
    public int? AddressId { get; set; }

    [ForeignKey(nameof(AddressId))]
    public virtual Address Address { get; set; }

    [ForeignKey("UserId")]
    public virtual User User { get; set; }

    public virtual ICollection<AgentCampaign> AgentCampaigns { get; set; }
}
