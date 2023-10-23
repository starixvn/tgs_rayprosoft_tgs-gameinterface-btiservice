using Tgs.GameInterface.BTiService.Core.Models;

namespace Tgs.GameInterface.BTiService.Core;

public interface IGameDataApiClient
{
    IAsyncEnumerable<BettingHistoryBet> BettingHistoryAsync(Uri apiUrl, string token, DateTime startTime, DateTime endTime, CancellationToken cancellationToken = default);

    [Obsolete("已過時，請改使用GetHistoryBetsPagingAsync")]
    IAsyncEnumerable<BettingHistoryBet> BettingHistoryPaginationAsync(Uri apiUrl, string token, DateTime startTime, DateTime endTime, int page = 0, int pageLimit = 1000, CancellationToken cancellationToken = default);

    IAsyncEnumerable<BetSettled> GetHistoryBetsPagingAsync(Uri apiUrl, string token, DateTimeOffset startTime, DateTimeOffset endTime, int page = 0, int pageLimit = 1000, CancellationToken cancellationToken = default);

    IAsyncEnumerable<BetUnsettled> GetOpenBetsPagingAsync(Uri apiUrl, string token, DateTimeOffset startTime, DateTimeOffset endTime, CancellationToken cancellationToken = default);

    [Obsolete("已過時，請改使用GetOpenBetsPagingAsync")]
    IAsyncEnumerable<OpenBet> OpenBetsAsync(Uri apiUrl, string token, DateTime startTime, DateTime endTime, CancellationToken cancellationToken = default);

    ValueTask<string> GetTokenAsync(Uri apiUrl, string agentUserName, string agentPassword, CancellationToken cancellationToken = default);
}
