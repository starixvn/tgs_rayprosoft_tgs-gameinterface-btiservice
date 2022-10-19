using System.Reflection;
using Tgs.GameInterface.BTiService.Core;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBTiServiceCore(
        this IServiceCollection services,
        Action<GameInterfaceSettingOptions, IServiceProvider> gameInterfaceSetting)
        => services
            .AddOptions<GameInterfaceSettingOptions>()
            .Configure(gameInterfaceSetting)
            .Services
            .AddHttpClient<IGameApiClient, GameApiClient>()
            .Services
            .AddHttpClient<IGameDataApiClient, GameDataApiClient>()
            .Services
            .RegisterAllServiceUtilityTypes(
                Assembly.GetExecutingAssembly(),
                ServiceLifetime.Transient);
}
