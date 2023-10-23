namespace Tgs.GameInterface.BTiService.ViewModels;

public record GameWagerUnsettledViewModel(
    Uri DataApiUrl,
    string AgentUserName,
    string AgentPassword,
    DateTimeOffset StartTime,
    DateTimeOffset EndTime);
