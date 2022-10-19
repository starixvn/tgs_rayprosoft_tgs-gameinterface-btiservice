namespace Tgs.GameInterface.BTiService.ViewModels;

public record GameHistoryUnsettledViewModel(
    Uri TokenApiUrl,
    Uri OpenBetsApiUrl,
    string AgentUserName,
    string AgentPassword,
    DateTimeOffset StartTime,
    DateTimeOffset EndTime);
