using Tgs.ServiceUtility;

namespace Tgs.GameInterface.BTiService.Core.ApplicationServices;

internal class GameUrlGetService : IGetService<GameUrlQuery, GameUrlResult>
{    
    private readonly IGameApiClient _client;
    private readonly ICommandService<CreatePlayerCommand> _createPlayerCommandService;

    public GameUrlGetService(        
        IGameApiClient client,
        ICommandService<CreatePlayerCommand> createPlayerCommandService)
    {        
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _createPlayerCommandService = createPlayerCommandService ?? throw new ArgumentNullException(nameof(createPlayerCommandService));
    }

    public async ValueTask<GameUrlResult> GetAsync(GameUrlQuery query, CancellationToken cancellationToken = default)
    {
        var authTokenResult = await _client
            .GetCustomerAuthTokenAsync(query.ApiUrl,query.AgentUserName,query.AgentPassword,query.PlayerCode, cancellationToken)
            .ConfigureAwait(false);



      
        var response = await _client
            .GetGameUrlAsync(
                query.ApiUrl,
                operatorToken: authTokenResult.authToken,
                query.IsMobile,
                oddsStyleId: null,
                sportId: null,
                leagueId: null,
                eventId: null,
                api: null,
                query.Language,                
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return new GameUrlResult(
                true,
                GameUrl: response?.Url ?? string.Empty);
    }
}
