using Microsoft.Extensions.Options;
using Tgs.GameInterface.BTiService.Client;
using Tgs.GameInterface.BTiService.Protos;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGameInterfaceBTiServiceClient(
        this IServiceCollection services,
        Action<GameInterfaceBTiClientOptions, IServiceProvider> configure)
        => services
            .AddOptions<GameInterfaceBTiClientOptions>()
            .Configure(configure)
            .Services
            .AddGrpcClient<GameInterfaceBTiService.GameInterfaceBTiServiceClient>((sp, configureClient) =>
            {
                var options = sp.GetRequiredService<IOptions<GameInterfaceBTiClientOptions>>().Value;
                configureClient.Address = options.BaseUrl;
            })
            .ConfigureChannel((sp, configureChannel) =>
            {
                var handler = new SocketsHttpHandler
                {
                    PooledConnectionIdleTimeout = Timeout.InfiniteTimeSpan,
                    KeepAlivePingDelay = TimeSpan.FromSeconds(60),
                    KeepAlivePingTimeout = TimeSpan.FromSeconds(30),
                    EnableMultipleHttp2Connections = true
                };
                configureChannel.HttpHandler = handler;
            })
            .AddInterceptor<LoggingInterceptor>()
            .Services
            .AddSingleton<DiagnosticsInterceptorOptions>()
            .AddSingleton<LoggingInterceptor>()
            .AddTransient<IGameInterfaceBTiClient, GameInterfaceBTiClient>();
}
