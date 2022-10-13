using Tgs.ServiceUtility;

namespace Tgs.GameInterface.BTiService.Core.ApplicationServices;

internal class FundOutGetService : IGetService<FundOutQuery, FundOutResult>
{
    private readonly IGameApiClient _client;

    public FundOutGetService(IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async ValueTask<FundOutResult> GetAsync(FundOutQuery query, CancellationToken cancellationToken = default)
    {
        var result = await _client.TransferToWHLAsync(
            apiUrl: query.ApiUrl,
            agentUserName: query.AgentUserName,
            agentPassword: query.AgentPassword,
            playerCode: query.PlayerCode,
            transactionId: query.TransactionId,
            amount: query.Amount,
            cancellationToken: cancellationToken).ConfigureAwait(false);

        return new FundOutResult(
            Success: result.Success,
            TransactionId: result.TransactionId.ToString(),
            BalanceAfter: result.BalanceAfter);
    }
}
