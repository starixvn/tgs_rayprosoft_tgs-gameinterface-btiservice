using Microsoft.Extensions.Options;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.BTiService.Core.ApplicationServices;

internal class PlayerCreateCommandService(
    IOptions<GameInterfaceSettingOptions> options,
    IGameApiClient client) : ICommandService<PlayerCreateCommand>
{
    private readonly GameInterfaceSettingOptions _gameInterfaceSetting = options.Value;

    public ValueTask ExecuteAsync(PlayerCreateCommand command, CancellationToken cancellationToken = default)
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

        return client.CreateNewUserAsync(
            apiUrl: command.ApiUrl,
            agentUserName: command.AgentUserName,
            agentPassword: command.AgentPassword,
            merchantCustomerCode: command.MerchantCustomerCode,
            currencyCode: command.Currency,
            firstName: command.BrandCode,
            lastName: command.MerchantCustomerCode,
            countryCode: mapping.CountryCode,
            city: mapping.City,
            customerDefaultLanguage: mapping.LanguageCode,
            cancellationToken);
    }
}
