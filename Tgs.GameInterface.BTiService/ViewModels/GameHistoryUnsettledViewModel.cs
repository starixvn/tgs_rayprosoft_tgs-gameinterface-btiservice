namespace Tgs.GameInterface.BTiService.ViewModels;

public record GameHistoryUnsettledViewModel(
    Uri TokenApiUrl,
    Uri OpenBetsApiUrl,
    string AgentUserName,
    string AgentPassword,
    DateTime StartTime,
    DateTime EndTime);
