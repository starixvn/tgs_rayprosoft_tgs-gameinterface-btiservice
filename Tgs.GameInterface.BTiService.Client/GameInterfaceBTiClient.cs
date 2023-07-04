using System.Runtime.CompilerServices;
using Google.Protobuf.WellKnownTypes;
using Tgs.GameInterface.BTiService.Protos;

namespace Tgs.GameInterface.BTiService.Client;

internal class GameInterfaceBTiClient : IGameInterfaceBTiClient
{
    private readonly GameInterfaceBTiService.GameInterfaceBTiServiceClient _client;

    public GameInterfaceBTiClient(GameInterfaceBTiService.GameInterfaceBTiServiceClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async ValueTask<string> CheckingServiceAsync(string machineName, CancellationToken cancellationToken = default)
    {
        var request = new CheckingServiceRequest
        {
            MachineName = machineName,
        };

        var reply = await _client
            .CheckingServiceAsync(
                request: request,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return reply.MachineName;
    }

    public async ValueTask<FundReply> FundInAsync(
        Uri apiUrl,
        string agentUserName,
        string agentPassword,
        string merchantCustomerCode,
        string refTransactionCode,
        decimal amount,
        CancellationToken cancellationToken = default)
    {
        var request = new FundRequest()
        {
            ApiUrl = apiUrl.ToString(),
            AgentUserName = agentUserName,
            AgentPassword = agentPassword,
            MerchantCustomerCode = merchantCustomerCode,
            RefTransactionCode = refTransactionCode,
            Amount = amount
        };

        return await _client
                .FundInAsync(
                    request: request,
                    cancellationToken: cancellationToken)
                .ConfigureAwait(false);
    }

    public async ValueTask<FundReply> FundOutAsync(
        Uri apiUrl,
        string agentUserName,
        string agentPassword,
        string merchantCustomerCode,
        string refTransactionCode,
        decimal amount,
        CancellationToken cancellationToken = default)
    {
        var request = new FundRequest()
        {
            ApiUrl = apiUrl.ToString(),
            AgentUserName = agentUserName,
            AgentPassword = agentPassword,
            MerchantCustomerCode = merchantCustomerCode,
            RefTransactionCode = refTransactionCode,
            Amount = amount
        };

        return await _client
                .FundOutAsync(
                    request: request,
                    cancellationToken: cancellationToken)
                .ConfigureAwait(false);
    }

    public async ValueTask<bool> FundStatusAsync(
        Uri apiUrl,
        string agentUserName,
        string agentPassword,
        string refTransactionCode,
        CancellationToken cancellationToken = default)
    {
        var request = new FundStatusRequest()
        {
            ApiUrl = apiUrl.ToString(),
            AgentUserName = agentUserName,
            AgentPassword = agentPassword,
            RefTransactionCode = refTransactionCode
        };

        var reply = await _client
                .FundStatusAsync(
                    request: request,
                    cancellationToken: cancellationToken)
                .ConfigureAwait(false);

        return reply.IsOrderExist;
    }

    public async ValueTask<GameAuthTokenReply> GameAuthTokenAsync(
        Uri apiUrl,
        string agentUserName,
        string agentPassword,
        string merchantCustomerCode,
        CancellationToken cancellationToken = default)
    {
        var request = new GameAuthTokenRequest()
        {
            ApiUrl = apiUrl.ToString(),
            AgentUserName = agentUserName,
            AgentPassword = agentPassword,
            MerchantCustomerCode = merchantCustomerCode
        };

        return await _client
                .GameAuthTokenAsync(
                    request: request,
                    cancellationToken: cancellationToken)
                .ConfigureAwait(false);
    }

    public async IAsyncEnumerable<GameHistorySettled> GameHistorySettledAsync(
        Uri tokenApiUrl,
        Uri historyBetsPagingApiUrl,
        string agentUserName,
        string agentPassword,
        DateTime startTime,
        DateTime endTime,
        int page = 0,
        int pageLimit = 1000,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var request = new GameHistorySettledRequest()
        {
            TokenApiUrl = tokenApiUrl.ToString(),
            HistoryBetsPagingApiUrl = historyBetsPagingApiUrl.ToString(),
            AgentUserName = agentUserName,
            AgentPassword = agentPassword,
            StartTime = startTime.ToTimestamp(),
            EndTime = endTime.ToTimestamp(),
            Page = page,
            PageLimit = pageLimit
        };

        var reply = await _client
                .GameHistorySettledAsync(
                    request: request,
                    cancellationToken: cancellationToken)
                .ConfigureAwait(false);

        if (!reply.Items.Any())
        {
            yield break;
        }

        foreach (var item in reply.Items)
        {
            yield return item;
        }
    }

    public async IAsyncEnumerable<GameHistoryUnsettled> GameHistoryUnsettledAsync(
        Uri tokenApiUrl,
        Uri openBetsApiUrl,
        string agentUserName,
        string agentPassword,
        DateTime startTime,
        DateTime endTime,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var request = new GameHistoryUnsettledRequest()
        {
            TokenApiUrl = tokenApiUrl.ToString(),
            OpenBetsApiUrl = openBetsApiUrl.ToString(),
            AgentUserName = agentUserName,
            AgentPassword = agentPassword,
            StartTime = startTime.ToTimestamp(),
            EndTime = endTime.ToTimestamp()
        };

        var reply = await _client
                .GameHistoryUnsettledAsync(
                    request: request,
                    cancellationToken: cancellationToken)
                .ConfigureAwait(false);

        if (!reply.Items.Any())
        {
            yield break;
        }

        foreach (var item in reply.Items)
        {
            yield return item;
        }
    }

    public async ValueTask<PlayerBalanceReply> PlayerBalanceAsync(
        Uri apiUrl,
        string agentUserName,
        string agentPassword,
        string merchantCustomerCode,
        CancellationToken cancellationToken)
    {
        var request = new PlayerBalanceRequest()
        {
            ApiUrl = apiUrl.ToString(),
            AgentUserName = agentUserName,
            AgentPassword = agentPassword,
            MerchantCustomerCode = merchantCustomerCode
        };

        return await _client
                .PlayerBalanceAsync(
                    request: request,
                    cancellationToken: cancellationToken)
                .ConfigureAwait(false);
    }

    public async ValueTask PlayerCreateAsync(
        Uri apiUrl,
        string agentUserName,
        string agentPassword,
        string brandCode,
        string merchantCustomerCode,
        string currency,
        CancellationToken cancellationToken)
    {
        var request = new PlayerCreateRequest()
        {
            ApiUrl = apiUrl.ToString(),
            AgentUserName = agentUserName,
            AgentPassword = agentPassword,
            BrandCode = brandCode,
            MerchantCustomerCode = merchantCustomerCode,
            Currency = currency
        };

        _ = await _client
                .PlayerCreateAsync(
                    request: request,
                    cancellationToken: cancellationToken)
                .ConfigureAwait(false);
    }
}
