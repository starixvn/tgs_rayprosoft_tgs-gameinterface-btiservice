using Tgs.ServiceUtility;

namespace Tgs.GameInterface.BTiService.Core.ApplicationServices;

internal class BalanceGetService : IGetService<BalanceQuery, BalanceResult>
{
    private readonly IGameApiClient _client;

    public BalanceGetService(IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async ValueTask<BalanceResult> GetAsync(BalanceQuery query, CancellationToken cancellationToken = default)
    {
        var result = await _client.GetCustomerByMerchantCodeAsync(
            apiUrl: query.ApiUrl,
            agentUserName: query.AgentUserName,
            agentPassword: query.AgentPassword,
            merchantCustomerCode: query.PlayerCode,
            cancellationToken: cancellationToken).ConfigureAwait(false);

        return new BalanceResult(
             IsPlayerExist: result.IsPlayerExist,
             Balance: result.Balance);
    }
}
