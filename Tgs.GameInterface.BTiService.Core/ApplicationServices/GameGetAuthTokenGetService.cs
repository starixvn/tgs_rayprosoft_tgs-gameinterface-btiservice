using Tgs.ServiceUtility;

namespace Tgs.GameInterface.BTiService.Core.ApplicationServices;

internal class GameGetAuthTokenGetService : IGetService<GameGetAuthTokenQuery, GameGetAuthTokenResult>
{
    private readonly IGameApiClient _client;

    public GameGetAuthTokenGetService(IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async ValueTask<GameGetAuthTokenResult> GetAsync(GameGetAuthTokenQuery query, CancellationToken cancellationToken = default)
    {
        var result = await _client
                .GetCustomerAuthTokenAsync(
                    apiUrl: query.ApiUrl,
                    agentUserName: query.AgentUserName,
                    agentPassword: query.AgentPassword,
                    merchantCustomerCode: query.MerchantCustomerCode,
                    cancellationToken: cancellationToken
                    )
                .ConfigureAwait(false);

        return new GameGetAuthTokenResult(
            IsPlayerExist: result.IsPlayerExist,
            AuthToken: result.AuthToken);
    }
}
