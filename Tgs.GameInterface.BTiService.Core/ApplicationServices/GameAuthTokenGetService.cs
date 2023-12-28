using Tgs.ServiceUtility;

namespace Tgs.GameInterface.BTiService.Core.ApplicationServices;

internal class GameAuthTokenGetService(IGameApiClient client) : IGetService<GameAuthTokenQuery, GameAuthTokenResult>
{
    public async ValueTask<GameAuthTokenResult> GetAsync(GameAuthTokenQuery query, CancellationToken cancellationToken = default)
    {
        var result = await client.GetCustomerAuthTokenAsync(
            apiUrl: query.ApiUrl,
            agentUserName: query.AgentUserName,
            agentPassword: query.AgentPassword,
            merchantCustomerCode: query.MerchantCustomerCode,
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return new GameAuthTokenResult(
            IsPlayerExist: result.IsPlayerExist,
            AuthToken: result.AuthToken);
    }
}
