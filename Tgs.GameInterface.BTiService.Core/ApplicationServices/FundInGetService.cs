using Tgs.ServiceUtility;

namespace Tgs.GameInterface.BTiService.Core.ApplicationServices;

internal class FundInGetService : IGetService<FundInQuery, FundInResult>
{
    private readonly IGameApiClient _client;

    public FundInGetService(IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async ValueTask<FundInResult> GetAsync(FundInQuery query, CancellationToken cancellationToken = default)
    {
        var result = await _client.TransferFromWHLAsync(
            apiUrl: query.ApiUrl,
            agentUserName: query.AgentUserName,
            agentPassword: query.AgentPassword,
            playerCode: query.PlayerCode,
            transactionId: query.TransactionId,
            amount: query.Amount,
            cancellationToken: cancellationToken).ConfigureAwait(false);

        return new FundInResult(
            Success: result.Success,
            TransactionId: result.TransactionId.ToString(),
            BalanceAfter: result.BalanceAfter);
    }
}
