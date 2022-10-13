using Tgs.GameInterface.BTiService.Protos;

namespace Tgs.GameInterface.BTiService.Client;

public interface IGameInterfaceBTiServiceGrpc
{
    ValueTask CreatePlayerAsync(Uri apiUrl, string agentId, string agentKey, string playerCode, CancellationToken cancellationToken = default);

    ValueTask<GameInterfaceBTiServiceFundReply> FundInAsync(Uri apiUrl, string agentId, string agentKey, string playerCode, string transactionId, decimal amount, CancellationToken cancellationToken = default);

    ValueTask<bool> FundOrderCheckingAsync(Uri apiUrl, string agentId, string agentKey, string transactionId, CancellationToken cancellationToken = default);

    ValueTask<GameInterfaceBTiServiceFundReply> FundOutAsync(Uri apiUrl, string agentId, string agentKey, string playerCode, string transactionId, decimal amount, CancellationToken cancellationToken = default);

    ValueTask<GameInterfaceBTiServiceBalanceReply> GetBalanceAsync(Uri apiUrl, string agentId, string agentKey, string playerCode, CancellationToken cancellationToken = default);

    ValueTask<GameInterfaceBTiServiceGamePullRecordReply> GetGamePullRecordAsync(Uri apiUrl, string agentId, string agentKey, DateTimeOffset startTime, DateTimeOffset endTime, int page = 1, int pageLimit = 100, CancellationToken cancellationToken = default);

    ValueTask<GameInterfaceBTiServiceGameUrlReply> GetGameUrlAsync(Uri apiUrl, string agentId, string agentKey, string playerCode, string externalGameId, string language, CancellationToken cancellationToken = default);
}
