using System.Diagnostics;

namespace Tgs.GameInterface.BTiService.Client;

public class DiagnosticsInterceptorOptions
{
    public ActivitySource InterceptorActivitySource { get; set; } =
        new ActivitySource($"Tgs.GameInterface.BTiService.Client.DiagnosticsInterceptor");
}
