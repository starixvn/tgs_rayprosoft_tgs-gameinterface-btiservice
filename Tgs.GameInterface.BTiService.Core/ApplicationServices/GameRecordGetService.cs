using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.BTiService.Core.ApplicationServices;

internal class GameRecordGetService : IQueryService<GameRecordQuery, GameRecordResult>
{
    private readonly IGameApiClient _client;
    private readonly ILogger<GameRecordGetService> _logger;

    public GameRecordGetService(
        ILogger<GameRecordGetService> logger,
        IGameApiClient client)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async IAsyncEnumerable<GameRecordResult> QueryAsync(GameRecordQuery query, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var result = await _client.BettinghistoryAsync(
            apiUrl: query.ApiUrl,   
            agentUserName: query.AgentUserName,
            agentPassword: query.AgentPassword,
            startTime: query.StartTime.UtcDateTime,
            endTime: query.EndTime.UtcDateTime,            
            cancellationToken: cancellationToken).ConfigureAwait(false);

        //if (!result.Success)
        //{
        //    _logger.LogError("Logon:{logon} | Message:{message}", DateTime.UtcNow.ToString("O"), result.Message ?? string.Empty);
        //    yield break;
        //}

        //if (result.Transactions is null || !result.Transactions.Any())
        //{
        //    yield break;
        //}

        foreach (var transaction in result.Transcations.Bets)
        {
            yield return new GameRecordResult(
                         //Account: transaction.Account,
                         //WagersId: transaction.WagersId,
                         //GameId: transaction.GameId,
                         //WagersTime: transaction.WagersTime,
                         //BetAmount: transaction.BetAmount,
                         //PayoffTime: transaction.PayoffTime,
                         //PayoffAmount: transaction.PayoffAmount,
                         //Status: transaction.Status,
                         //SettlementTime: transaction.SettlementTime,
                         //GameCategoryId: transaction.GameCategoryId,
                         //Type: transaction.Type,
                         //AgentId: transaction.AgentId
                         );
        }
    }
}
