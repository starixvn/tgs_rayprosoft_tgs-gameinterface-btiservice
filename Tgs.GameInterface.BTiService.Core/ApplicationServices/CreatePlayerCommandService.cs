using Tgs.ServiceUtility;

namespace Tgs.GameInterface.BTiService.Core.ApplicationServices;

internal class CreatePlayerCommandService : ICommandService<CreatePlayerCommand>
{
    private readonly IGameApiClient _client;
    private readonly GameInterfaceSettingOptions _gameInterfaceSetting;

    public CreatePlayerCommandService(
        IOptions<GameInterfaceSettingOptions> options,
        IGameApiClient client)
    {
        _gameInterfaceSetting = options.Value;
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async ValueTask ExecuteAsync(CreatePlayerCommand command, CancellationToken cancellationToken = default)
    {
        var mapping = _gameInterfaceSetting
            .CurrencyMappings
            .Where(x => x.CurrencyCode.Equals(command.Currency, StringComparison.OrdinalIgnoreCase))
            .FirstOrDefault() ??
            new CurrencyMapping()
            {
                CurrencyCode = command.Currency,
                CountryCode = "CN",
                LanguageCode = "zh-cn",
            };

        await _client.CreateNewUserAsync(
            command.ApiUrl,
            command.AgentUserName,
            command.AgentPassword,
            command.PlayerCode,
            command.Currency,
            mapping.CountryCode,
            mapping.LanguageCode,
            cancellationToken);
    }
}
