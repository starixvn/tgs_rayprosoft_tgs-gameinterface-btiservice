namespace Tgs.GameInterface.BTiService.ViewModels;

public record GameHistorySettledViewModel(
    Uri TokenApiUrl,
    Uri HistoryBetsPagingApiUrl,
    string AgentUserName,
    string AgentPassword,
    DateTime StartTime,
    DateTime EndTime,
    int Page = 0,
    int PageLimit = 1000);
