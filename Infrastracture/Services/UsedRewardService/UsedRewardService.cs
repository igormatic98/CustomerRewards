using Auth.Services;
using CustomerRewards.Company.Entities;
using CustomerRewards.Infrastructure;

namespace Infrastracture.Services.UsedRewardService;

public class UsedRewardService
{
    private readonly DatabaseContext databaseContext;
    private readonly TokenReaderService tokenReaderService;

    public UsedRewardService(DatabaseContext databaseContext, TokenReaderService tokenReaderService)
    {
        this.databaseContext = databaseContext;
        this.tokenReaderService = tokenReaderService;
    }

    public async Task CustomerUsedReward(int customerId, decimal usedAmount)
    {
        var activeCampaignId = tokenReaderService.GetCampaignId();
        if (activeCampaignId == 0)

            throw new Exception("No active campaign. Reward expired.");

        await databaseContext.UsedRewards.AddAsync(
            new UsedReward
            {
                CustomerId = customerId,
                CampaignId = activeCampaignId,
                UsedDate = DateTime.Now,
                UsedAmount = usedAmount,
            }
        );
        await databaseContext.SaveChangesAsync();
    }
}
