using System.Runtime.CompilerServices;
using Google.Protobuf.WellKnownTypes;
using Tgs.GameInterface.BTiService.Protos;

namespace Tgs.GameInterface.BTiService.Client;

internal class GameInterfaceBTiClient(
    GameInterfaceBTiService.GameInterfaceBTiServiceClient client) : IGameInterfaceBTiClient
{
    public async ValueTask<string> CheckingServiceAsync(string machineName, CancellationToken cancellationToken = default)
    {
        var request = new CheckingServiceRequest
        {
            MachineName = machineName,
        };

        var reply = await client
            .CheckingServiceAsync(
                request: request,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return reply.MachineName;
    }

    public async ValueTask<FundReply> FundInAsync(Uri apiUrl, string agentUserName, string agentPassword, string merchantCustomerCode, string refTransactionCode, decimal amount, CancellationToken cancellationToken = default)
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

        return await client
                .FundInAsync(
                    request: request,
                    cancellationToken: cancellationToken)
                .ConfigureAwait(false);
    }

    public async ValueTask<FundReply> FundOutAsync(Uri apiUrl, string agentUserName, string agentPassword, string merchantCustomerCode, string refTransactionCode, decimal amount, CancellationToken cancellationToken = default)
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

        return await client
                .FundOutAsync(
                    request: request,
                    cancellationToken: cancellationToken)
                .ConfigureAwait(false);
    }

    public async ValueTask<bool> FundStatusAsync(Uri apiUrl, string agentUserName, string agentPassword, string refTransactionCode, CancellationToken cancellationToken = default)
    {
        var request = new FundStatusRequest()
        {
            ApiUrl = apiUrl.ToString(),
            AgentUserName = agentUserName,
            AgentPassword = agentPassword,
            RefTransactionCode = refTransactionCode
        };

        var reply = await client
                .FundStatusAsync(
                    request: request,
                    cancellationToken: cancellationToken)
                .ConfigureAwait(false);

        return reply.Status;
    }

    public async ValueTask<GameAuthTokenReply> GameAuthTokenAsync(Uri apiUrl, string agentUserName, string agentPassword, string merchantCustomerCode, CancellationToken cancellationToken = default)
    {
        var request = new GameAuthTokenRequest()
        {
            ApiUrl = apiUrl.ToString(),
            AgentUserName = agentUserName,
            AgentPassword = agentPassword,
            MerchantCustomerCode = merchantCustomerCode
        };

        return await client
                .GameAuthTokenAsync(
                    request: request,
                    cancellationToken: cancellationToken)
                .ConfigureAwait(false);
    }

    public async IAsyncEnumerable<GameWagerInfo> GameWagerSettledAsync(Uri dataApiUrl, string agentUserName, string agentPassword, DateTimeOffset startTime, DateTimeOffset endTime, int page = 0, int pageLimit = 1000, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var request = new GameWagerSettledRequest()
        {
            DataApiUrl = dataApiUrl.ToString(),
            AgentUserName = agentUserName,
            AgentPassword = agentPassword,
            StartTime = startTime.ToTimestamp(),
            EndTime = endTime.ToTimestamp(),
            Page = page,
            PageLimit = pageLimit
        };

        var reply = await client.GameWagerSettledAsync(
            request: request,
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        var items = reply.Items ?? Enumerable.Empty<GameWagerInfo>();
        if (!items.Any())
        {
            yield break;
        }

        foreach (var item in items)
        {
            yield return item;
        }
    }

    public async IAsyncEnumerable<GameWagerInfo> GameWagerUnsettledAsync(Uri dataApiUrl, string agentUserName, string agentPassword, DateTimeOffset startTime, DateTimeOffset endTime, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var request = new GameWagerUnsettledRequest()
        {
            DataApiUrl = dataApiUrl.ToString(),
            AgentUserName = agentUserName,
            AgentPassword = agentPassword,
            StartTime = startTime.ToTimestamp(),
            EndTime = endTime.ToTimestamp()
        };

        var reply = await client.GameWagerUnsettledAsync(
            request: request,
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        var items = reply.Items ?? Enumerable.Empty<GameWagerInfo>();
        if (!items.Any())
        {
            yield break;
        }

        foreach (var item in items)
        {
            yield return item;
        }
    }

    public async ValueTask<PlayerBalanceReply> PlayerBalanceAsync(Uri apiUrl, string agentUserName, string agentPassword, string merchantCustomerCode, CancellationToken cancellationToken)
    {
        var request = new PlayerBalanceRequest()
        {
            ApiUrl = apiUrl.ToString(),
            AgentUserName = agentUserName,
            AgentPassword = agentPassword,
            MerchantCustomerCode = merchantCustomerCode
        };

        return await client
                .PlayerBalanceAsync(
                    request: request,
                    cancellationToken: cancellationToken)
                .ConfigureAwait(false);
    }

    public async ValueTask PlayerCreateAsync(Uri apiUrl, string agentUserName, string agentPassword, string brandCode, string merchantCustomerCode, string currency, CancellationToken cancellationToken)
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

        _ = await client
                .PlayerCreateAsync(
                    request: request,
                    cancellationToken: cancellationToken)
                .ConfigureAwait(false);
    }
}
