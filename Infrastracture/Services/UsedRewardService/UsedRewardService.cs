using CustomerRewards.Infrastructure;

namespace Infrastracture.Services.UsedRewardService;

public class UsedRewardService
{
    private readonly DatabaseContext databaseContext;

    public UsedRewardService(DatabaseContext context)
    {
        this.databaseContext = databaseContext;
    }

    public async Task CustomerUsedReward() { }
}
