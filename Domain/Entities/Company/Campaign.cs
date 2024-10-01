using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CustomerRewards.Company.Entities;

[PrimaryKey(nameof(Id))]
public class Campaign
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int CompanyId { get; set; }

    [ForeignKey(nameof(CompanyId))]
    public Company Company { get; set; }

    public ICollection<AgentCampaign> AgentCampaigns { get; set; }
}
