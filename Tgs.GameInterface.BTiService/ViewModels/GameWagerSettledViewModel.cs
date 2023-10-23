namespace Tgs.GameInterface.BTiService.ViewModels;

public record GameWagerSettledViewModel(
    Uri DataApiUrl,
    string AgentUserName,
    string AgentPassword,
    DateTimeOffset StartTime,
    DateTimeOffset EndTime,
    int Page = 0,
    int PageLimit = 1000);
