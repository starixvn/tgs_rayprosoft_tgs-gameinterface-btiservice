using Tgs.GameInterface.BTiService.Core.Models;

namespace Tgs.GameInterface.BTiService.Core;

public interface IGameDataApiClient
{
    IAsyncEnumerable<BettingHistoryBet> BettingHistoryAsync(
        Uri apiUrl,
        string token,
        DateTime startTime,
        DateTime endTime,
        CancellationToken cancellationToken = default);

    IAsyncEnumerable<BettingHistoryBet> BettingHistoryPaginationAsync(
        Uri apiUrl,
        string token,
        DateTime startTime,
        DateTime endTime,
        int page = 0,
        int pageLimit = 1000,
        CancellationToken cancellationToken = default);

    IAsyncEnumerable<OpenBet> OpenBetsAsync(
        Uri apiUrl,
        string token,
        DateTime startTime,
        DateTime endTime,
        CancellationToken cancellationToken = default);

    ValueTask<string> GetTokenAsync(
        Uri apiUrl,
        string agentUserName,
        string agentPassword,
        CancellationToken cancellationToken = default);
}
