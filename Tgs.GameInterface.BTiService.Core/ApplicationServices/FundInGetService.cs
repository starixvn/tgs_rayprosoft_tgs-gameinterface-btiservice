using Tgs.ServiceUtility;

namespace Tgs.GameInterface.BTiService.Core.ApplicationServices;

internal class FundInGetService : IGetService<FundInQuery, FundResult>
{
    private readonly IGameApiClient _client;

    public FundInGetService(IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async ValueTask<FundResult> GetAsync(FundInQuery query, CancellationToken cancellationToken = default)
    {
        var result = await _client.TransferFromWHLAsync(
            apiUrl: query.ApiUrl,
            agentUserName: query.AgentUserName,
            agentPassword: query.AgentPassword,
            merchantCustomerCode: query.MerchantCustomerCode,
            refTransactionCode: query.RefTransactionCode,
            amount: query.Amount,
            cancellationToken: cancellationToken).ConfigureAwait(false);

        return new FundResult(
            RefTransactionCode: result.RefTransactionCode,
            TransactionId: result.TransactionId,
            Balance: result.Balance);
    }
}
