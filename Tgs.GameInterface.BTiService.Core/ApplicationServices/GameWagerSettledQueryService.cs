using System.Runtime.CompilerServices;
using Tgs.GameInterface.BTiService.Core.Models;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.BTiService.Core.ApplicationServices;

internal class GameWagerSettledQueryService(
    IGameDataApiClient client) : IQueryService<GameWagerSettledQuery, GameWagerResult>
{
    public async IAsyncEnumerable<GameWagerResult> QueryAsync(GameWagerSettledQuery query, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var token = await client.GetTokenAsync(
            apiUrl: query.DataApiUrl,
            agentUserName: query.AgentUserName,
            agentPassword: query.AgentPassword,
            cancellationToken)
            .ConfigureAwait(false);

        var betSettleds = await client.GetHistoryBetsPagingAsync(
            apiUrl: query.DataApiUrl,
            token: token,
            startTime: query.StartTime,
            endTime: query.EndTime,
            page: query.Page,
            pageLimit: query.PageLimit,
            cancellationToken: cancellationToken)
            .ToArrayAsync(cancellationToken)
            .ConfigureAwait(false)
            ?? Enumerable.Empty<BetSettled>();

        if (!betSettleds.Any())
        {
            yield break;
        }

        foreach (var betSettled in betSettleds)
        {
            bool isClosed = false;
            DateTimeOffset externalClosedOn = DateTime.UnixEpoch;
            decimal winLoss = 0;
            decimal settle = 0;

            if (!betSettled.BetStatus.Equals("Opened"))
            {
                isClosed = true;
                externalClosedOn = betSettled.BetSettledDate;
                winLoss = betSettled.PL;
                settle = betSettled.Return;
            }

            yield return new GameWagerResult(
                PlayerCode: betSettled.MerchantCustomerID,
                ExternalWagerId: betSettled.PurchaseID,
                IsClosed: isClosed,
                Status: betSettled.BetStatus,
                ExternalCreatedOn: betSettled.CreationDate,
                ExternalClosedOn: externalClosedOn,
                Bet: betSettled.TotalStake,
                ValidBet: betSettled.ValidStake,
                Settle: settle,
                WinLoss: winLoss,
                AdditionalData: betSettled.AdditionalData);
        }
    }
}
