using System.Runtime.CompilerServices;
using Tgs.GameInterface.BTiService.Core.Models;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.BTiService.Core.ApplicationServices;

internal class GameWagerUnsettledQueryService : IQueryService<GameWagerUnsettledQuery, GameWagerResult>
{
    private readonly IGameDataApiClient _client;

    public GameWagerUnsettledQueryService(
        IGameDataApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async IAsyncEnumerable<GameWagerResult> QueryAsync(GameWagerUnsettledQuery query, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var token = await _client.GetTokenAsync(
            apiUrl: query.DataApiUrl,
            agentUserName: query.AgentUserName,
            agentPassword: query.AgentPassword,
            cancellationToken)
            .ConfigureAwait(false);

        var betUnsettleds = await _client.GetOpenBetsPagingAsync(
            apiUrl: query.DataApiUrl,
            token: token,
            startTime: query.StartTime,
            endTime: query.EndTime,
            cancellationToken: cancellationToken)
            .ToArrayAsync(cancellationToken)
            .ConfigureAwait(false)
            ?? Enumerable.Empty<BetUnsettled>();

        if (!betUnsettleds.Any())
        {
            yield break;
        }

        foreach (var betUnsettled in betUnsettleds)
        {
            yield return new GameWagerResult(
                PlayerCode: betUnsettled.MerchantCustomerID,
                ExternalWagerId: betUnsettled.PurchaseID,
                IsClosed: false,
                Status: betUnsettled.BetStatus,
                ExternalCreatedOn: betUnsettled.CreationDate,
                ExternalClosedOn: DateTimeOffset.UnixEpoch,
                Bet: betUnsettled.TotalStake,
                ValidBet: betUnsettled.ValidStake,
                Settle: 0.0m,
                WinLoss: 0.0m,
                AdditionalData: betUnsettled.AdditionalData);
        }
    }
}
