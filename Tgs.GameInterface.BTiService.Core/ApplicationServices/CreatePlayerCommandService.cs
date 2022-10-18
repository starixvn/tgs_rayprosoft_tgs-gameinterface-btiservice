using Microsoft.Extensions.Options;
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

    public ValueTask ExecuteAsync(CreatePlayerCommand command, CancellationToken cancellationToken = default)
    {
        var mapping = _gameInterfaceSetting
            .CurrencyMappings
            .Where(x => x.CurrencyCode.Equals(command.Currency, StringComparison.OrdinalIgnoreCase))
            .FirstOrDefault() ??
            new CurrencyMapping()
            {
                CurrencyCode = command.Currency,
                CountryCode = "CN",
                LanguageCode = "zh",
                DisplayName = "China",
                City = "Beijing"
            };

        return _client.CreateNewUserAsync(
            apiUrl: command.ApiUrl,
            agentUserName: command.AgentUserName,
            agentPassword: command.AgentPassword,
            merchantCustomerCode: command.PlayerCode,
            currencyCode: command.Currency,
            firstName: command.Brand,
            lastName: command.PlayerCode,
            countryCode: mapping.CountryCode,
            city: mapping.City,
            customerDefaultLanguage: mapping.LanguageCode,
            cancellationToken);
    }
}
