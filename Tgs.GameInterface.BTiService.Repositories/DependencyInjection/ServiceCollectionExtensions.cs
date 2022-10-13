using Tgs.GameInterface.BTiService.Core;
using Tgs.GameInterface.BTiService.Repositories;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBTiServiceRepositories(this IServiceCollection services)
        => services
            .AddHttpClient<IGameApiClient, GameApiClient>()
            .Services;
}
