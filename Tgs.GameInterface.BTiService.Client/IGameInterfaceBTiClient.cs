using Tgs.GameInterface.BTiService.Protos;

namespace Tgs.GameInterface.BTiService.Client;

public interface IGameInterfaceBTiClient
{
    ValueTask<string> CheckingServiceAsync(
        string machineName,
        CancellationToken cancellationToken = default);

    ValueTask<FundReply> FundInAsync(
        Uri apiUrl,
        string agentUserName,
        string agentPassword,
        string merchantCustomerCode,
        string refTransactionCode,
        decimal amount,
        CancellationToken cancellationToken = default);

    ValueTask<FundReply> FundOutAsync(
        Uri apiUrl,
        string agentUserName,
        string agentPassword,
        string merchantCustomerCode,
        string refTransactionCode,
        decimal amount,
        CancellationToken cancellationToken = default);

    ValueTask<bool> FundStatusAsync(
        Uri apiUrl,
        string agentUserName,
        string agentPassword,
        string refTransactionCode,
        CancellationToken cancellationToken = default);

    ValueTask<GameAuthTokenReply> GameAuthTokenAsync(
        Uri apiUrl,
        string agentUserName,
        string agentPassword,
        string merchantCustomerCode,
        CancellationToken cancellationToken = default);

    IAsyncEnumerable<GameHistorySettled> GameHistorySettledAsync(
        Uri tokenApiUrl,
        Uri historyBetsPagingApiUrl,
        string agentUserName,
        string agentPassword,
        DateTime startTime,
        DateTime endTime,
        int page = 0,
        int pageLimit = 1000,
        CancellationToken cancellationToken = default);

    IAsyncEnumerable<GameHistoryUnsettled> GameHistoryUnsettledAsync(
        Uri tokenApiUrl,
        Uri openBetsApiUrl,
        string agentUserName,
        string agentPassword,
        DateTime startTime,
        DateTime endTime,
        CancellationToken cancellationToken = default);

    ValueTask<PlayerBalanceReply> PlayerBalanceAsync(
        Uri apiUrl,
        string agentUserName,
        string agentPassword,
        string merchantCustomerCode,
        CancellationToken cancellationToken = default);

    ValueTask PlayerCreateAsync(
        Uri apiUrl,
        string agentUserName,
        string agentPassword,
        string brandCode,
        string merchantCustomerCode,
        string currency,
        CancellationToken cancellationToken = default);
}
