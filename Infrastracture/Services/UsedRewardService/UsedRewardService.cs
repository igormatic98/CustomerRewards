using Auth.Services;
using CustomerRewards.Company.Entities;
using CustomerRewards.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Infrastracture.Services.UsedRewardService;

/// <summary>
/// Servis za unos kupovina, odnosno koji bonovi od strane kojih korisnika su iskoristeni
/// Pokrece ga prodavac
/// </summary>
public class UsedRewardService
{
    private readonly DatabaseContext databaseContext;
    private readonly TokenReaderService tokenReaderService;

    public UsedRewardService(DatabaseContext databaseContext, TokenReaderService tokenReaderService)
    {
        this.databaseContext = databaseContext;
        this.tokenReaderService = tokenReaderService;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="customerId">id kupca</param>
    /// <param name="usedAmount">koliko novca sada zeli da iskoristi za kupovinu</param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task CustomerUsedReward(int customerId, decimal usedAmount)
    {
        var activeCampaignId = tokenReaderService.GetCampaignId();

        if (activeCampaignId == 0)
        {
            throw new Exception("No active campaign. Reward expired.");
        }

        //obzirom da je sistem zamisljen da kupac nagradu moze iskorisitit na vise dijelova
        //potrebno je ograniciti da unos svih kupovina ne moze da bude veci od ukupne vrijednosti bona koju je kupac dobio
        var customerRewardsAmountInfo = await databaseContext.CustomersRewards
            .Where(cr => cr.CampaignId == activeCampaignId && cr.CustomerId == customerId)
            .Select(
                cr =>
                    new
                    {
                        RewardAmount = cr.RewardAmount,
                        totalUsedAmount = cr.UsedRewards.Sum(cr => cr.UsedAmount)
                    }
            )
            .FirstOrDefaultAsync();
        if (
            customerRewardsAmountInfo.totalUsedAmount + usedAmount
            > customerRewardsAmountInfo.RewardAmount
        )
        {
            throw new Exception("The total used amount exceeds the available reward.");
        }
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
