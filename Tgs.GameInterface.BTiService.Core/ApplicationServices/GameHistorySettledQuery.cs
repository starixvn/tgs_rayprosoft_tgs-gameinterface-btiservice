namespace Tgs.GameInterface.BTiService.Core.ApplicationServices;

public record GameHistorySettledQuery(
    Uri TokenApiUrl,
    Uri HistoryBetsPagingApiUrl,
    string AgentUserName,
    string AgentPassword,
    DateTime StartTime,
    DateTime EndTime,
    int Page = 0,
    int PageLimit = 1000);
