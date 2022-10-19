using System.Runtime.CompilerServices;
using System.Text.Json;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.BTiService.Core.ApplicationServices;

internal class GameHistorySettledQueryService : IQueryService<GameHistorySettledQuery, GameHistorySettledResult>
{
    private readonly IGameDataApiClient _client;

    public GameHistorySettledQueryService(
        IGameDataApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async IAsyncEnumerable<GameHistorySettledResult> QueryAsync(GameHistorySettledQuery query, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var token = await _client
                .GetTokenAsync(
                    apiUrl: query.TokenApiUrl,
                    agentUserName: query.AgentUserName,
                    agentPassword: query.AgentPassword,
                    cancellationToken)
                .ConfigureAwait(false);

        var bettingHistoryBets = await _client
                .BettingHistoryPaginationAsync(
                    apiUrl: query.HistoryBetsPagingApiUrl,
                    token: token,
                    startTime: query.StartTime,
                    endTime: query.EndTime,
                    page: query.Page,
                    pageLimit: query.PageLimit,
                    cancellationToken: cancellationToken)
                .ToArrayAsync(cancellationToken)
                .ConfigureAwait(false);

        if (!bettingHistoryBets.Any())
        {
            yield break;
        }

        foreach (var bettingHistoryBet in bettingHistoryBets)
        {
            bool isClosed = false;
            DateTimeOffset externalClosedOn = DateTime.UnixEpoch;
            decimal playerWinLoss = 0;
            decimal turnover = 0;
            decimal settle = 0;

            if (!bettingHistoryBet.BetStatus.Equals("Opened"))
            {
                isClosed = true;
                externalClosedOn = bettingHistoryBet.BetSettledDate;
                playerWinLoss = bettingHistoryBet.PL;
                turnover = bettingHistoryBet.ValidStake;
                settle = bettingHistoryBet.Return;
            }

            yield return new GameHistorySettledResult(
                PlayerCode: bettingHistoryBet.MerchantCustomerID,
                ExternalRoundId: bettingHistoryBet.PurchaseID,
                IsClosed: isClosed,
                Status: bettingHistoryBet.BetStatus,
                ExternalCreatedOn: bettingHistoryBet.CreationDate,
                ExternalClosedOn: externalClosedOn,
                Bet: bettingHistoryBet.TotalStake,
                ValidBet: bettingHistoryBet.ValidStake,
                PlayerWinLoss: playerWinLoss,
                Turnover: turnover,
                Settle: settle,
                AdditionalData: JsonSerializer.Serialize(bettingHistoryBet));
        }
    }
}
