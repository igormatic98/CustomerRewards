﻿namespace Infrastracture.Dtos;

/// <summary>
/// Dto za aktivnu kampanju, i agenta koji je zaduzen za nju
/// </summary>
public class ActiveCampaignDto
{
    public int AgentId { get; set; }
    public int CampaignId { get; set; }
}
