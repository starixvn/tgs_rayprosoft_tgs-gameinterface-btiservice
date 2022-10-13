namespace Tgs.GameInterface.BTiService.Core.ApplicationServices;

public record GameUrlQuery(
    Uri ApiUrl,
    string AgentUserName,
    string AgentPassword,
    string PlayerCode,
    string ExternalGameId,
    string Language,
    string Currency,
    bool IsMobile);
