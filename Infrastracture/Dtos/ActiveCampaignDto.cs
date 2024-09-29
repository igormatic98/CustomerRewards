using Azure.Core.Pipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastracture.Dtos;

public class ActiveCampaignDto
{
    public int AgentId { get; set; }
    public int CampaignId { get; set; }
}
