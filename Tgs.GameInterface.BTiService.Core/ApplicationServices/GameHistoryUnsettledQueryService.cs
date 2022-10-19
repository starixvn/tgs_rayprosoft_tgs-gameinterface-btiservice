using System.Runtime.CompilerServices;
using System.Text.Json;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.BTiService.Core.ApplicationServices;

internal class GameHistoryUnsettledQueryService : IQueryService<GameHistoryUnsettledQuery, GameHistoryUnsettledResult>
{
    private readonly IGameDataApiClient _client;

    public GameHistoryUnsettledQueryService(
        IGameDataApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async IAsyncEnumerable<GameHistoryUnsettledResult> QueryAsync(GameHistoryUnsettledQuery query, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var token = await _client
                .GetTokenAsync(
                    apiUrl: query.TokenApiUrl,
                    agentUserName: query.AgentUserName,
                    agentPassword: query.AgentPassword,
                    cancellationToken)
                .ConfigureAwait(false);

        var openBets = await _client
                .OpenBetsAsync(
                    apiUrl: query.OpenBetsApiUrl,
                    token: token,
                    startTime: query.StartTime.UtcDateTime,
                    endTime: query.EndTime.UtcDateTime,
                    cancellationToken: cancellationToken)
                .ToArrayAsync(cancellationToken)
                .ConfigureAwait(false);

        if (!openBets.Any())
        {
            yield break;
        }

        foreach (var openBet in openBets)
        {
            yield return new GameHistoryUnsettledResult(
                PlayerCode: openBet.MerchantCustomerID,
                ExternalRoundId: openBet.PurchaseID,
                ExternalCreatedOn: openBet.CreationDate,
                Bet: openBet.TotalStake,
                ValidBet: openBet.ValidStake,
                AdditionalData: JsonSerializer.Serialize(openBet));
        }
    }
}
