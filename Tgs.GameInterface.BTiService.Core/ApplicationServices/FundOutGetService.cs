using Tgs.ServiceUtility;

namespace Tgs.GameInterface.BTiService.Core.ApplicationServices;

internal class FundOutGetService : IGetService<FundOutQuery, FundResult>
{
    private readonly IGameApiClient _client;

    public FundOutGetService(IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async ValueTask<FundResult> GetAsync(FundOutQuery query, CancellationToken cancellationToken = default)
    {
        var result = await _client.TransferToWHLAsync(
            apiUrl: query.ApiUrl,
            agentUserName: query.AgentUserName,
            agentPassword: query.AgentPassword,
            merchantCustomerCode: query.PlayerCode,
            refTransactionCode: query.TransactionId,
            amount: query.Amount,
            cancellationToken: cancellationToken).ConfigureAwait(false);

        return new FundResult(
            TransactionId: result.TransactionId.ToString(),
            Balance: result.Balance);
    }
}
