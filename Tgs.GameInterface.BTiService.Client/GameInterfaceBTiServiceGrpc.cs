using Google.Protobuf.WellKnownTypes;
using Tgs.GameInterface.BTiService.Protos;

namespace Tgs.GameInterface.BTiService.Client;

internal class GameInterfaceBTiServiceGrpc : IGameInterfaceBTiServiceGrpc
{
    private readonly GameInterfaceBTiService.GameInterfaceBTiServiceClient _client;

    public GameInterfaceBTiServiceGrpc(GameInterfaceBTiService.GameInterfaceBTiServiceClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async ValueTask CreatePlayerAsync(Uri apiUrl, string agentId, string agentKey, string playerCode, CancellationToken cancellationToken = default)
        => await _client.CreatePlayerAsync(new GameInterfaceBTiServiceCreatePlayerRequest()
        {
            ApiUrl = apiUrl.ToString(),
            AgentUserName = agentId,
            AgentPassword = agentKey,
            PlayerCode = playerCode
        }, cancellationToken: cancellationToken).ConfigureAwait(false);

    public async ValueTask<GameInterfaceBTiServiceFundReply> FundInAsync(Uri apiUrl, string agentId, string agentKey, string playerCode, string transactionId, decimal amount, CancellationToken cancellationToken = default)
        => await _client.FundInAsync(new GameInterfaceBTiServiceFundRequest()
        {
            ApiUrl = apiUrl.ToString(),
            AgentUserName = agentId,
            AgentPassword = agentKey,
            PlayerCode = playerCode,
            TransactionId = transactionId,
            Amount = amount
        }, cancellationToken: cancellationToken).ConfigureAwait(false);

    public async ValueTask<bool> FundOrderCheckingAsync(Uri apiUrl, string agentId, string agentKey, string transactionId, CancellationToken cancellationToken = default)
    {
        var reply = await _client.FundOrderCheckingAsync(new GameInterfaceBTiServiceFundOrderCheckingRequest()
        {
            ApiUrl = apiUrl.ToString(),
            AgentUserName = agentId,
            AgentPassword = agentKey,
            TransactionId = transactionId
        }, cancellationToken: cancellationToken).ConfigureAwait(false);

        return reply.Exist;
    }

    public async ValueTask<GameInterfaceBTiServiceFundReply> FundOutAsync(Uri apiUrl, string agentId, string agentKey, string playerCode, string transactionId, decimal amount, CancellationToken cancellationToken = default)
        => await _client.FundOutAsync(new GameInterfaceBTiServiceFundRequest()
        {
            ApiUrl = apiUrl.ToString(),
            AgentUserName = agentId,
            AgentPassword = agentKey,
            PlayerCode = playerCode,
            TransactionId = transactionId,
            Amount = amount
        }, cancellationToken: cancellationToken).ConfigureAwait(false);

    public async ValueTask<GameInterfaceBTiServiceBalanceReply> GetBalanceAsync(Uri apiUrl, string agentId, string agentKey, string playerCode, CancellationToken cancellationToken = default)
        => await _client.GetBalanceAsync(new GameInterfaceBTiServiceBalanceRequest()
        {
            ApiUrl = apiUrl.ToString(),
            AgentUserName = agentId,
            AgentPassword = agentKey,
            PlayerCode = playerCode,
        }, cancellationToken: cancellationToken).ConfigureAwait(false);

    public async ValueTask<GameInterfaceBTiServiceGamePullRecordReply> GetGamePullRecordAsync(Uri apiUrl, string agentId, string agentKey, DateTimeOffset startTime, DateTimeOffset endTime, int page = 1, int pageLimit = 100, CancellationToken cancellationToken = default)
        => await _client.GetGamePullRecordAsync(new GameInterfaceBTiServiceGamePullRecordRequest()
        {
            ApiUrl = apiUrl.ToString(),
            AgentUserName = agentId,
            AgentPassword = agentKey,
            StartTime = startTime.ToTimestamp(),
            EndTime = endTime.ToTimestamp(),
            Page = page,
            PageLimit = pageLimit
        }, cancellationToken: cancellationToken).ConfigureAwait(false);

    public async ValueTask<GameInterfaceBTiServiceGameUrlReply> GetGameUrlAsync(Uri apiUrl, string agentId, string agentKey, string playerCode, string externalGameId, string language, CancellationToken cancellationToken = default)
        => await _client.GetGameUrlAsync(new GameInterfaceBTiServiceGameUrlRequest()
        {
            ApiUrl = apiUrl.ToString(),
            AgentUserName = agentId,
            AgentPassword = agentKey,
            PlayerCode = playerCode,
            ExternalGameId = externalGameId,
            Language = language
        }, cancellationToken: cancellationToken).ConfigureAwait(false);
}
