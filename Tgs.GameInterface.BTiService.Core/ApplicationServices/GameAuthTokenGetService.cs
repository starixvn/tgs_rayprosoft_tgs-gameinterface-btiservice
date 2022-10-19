using Tgs.ServiceUtility;

namespace Tgs.GameInterface.BTiService.Core.ApplicationServices;

internal class GameAuthTokenGetService : IGetService<GameAuthTokenQuery, GameAuthTokenResult>
{
    private readonly IGameApiClient _client;

    public GameAuthTokenGetService(IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async ValueTask<GameAuthTokenResult> GetAsync(GameAuthTokenQuery query, CancellationToken cancellationToken = default)
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

        return new GameAuthTokenResult(
            IsPlayerExist: result.IsPlayerExist,
            AuthToken: result.AuthToken);
    }
}
