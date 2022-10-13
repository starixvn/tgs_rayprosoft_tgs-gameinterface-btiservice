namespace Tgs.GameInterface.BTiService.ViewModels;

public record GameUrlQueryViewModel(
    Uri ApiUrl,
    string AgentUserName,
    string AgentPassword,
    string PlayerCode,
    string ExternalGameId,
    string Language,
    string Currency,
    bool IsMobile);
