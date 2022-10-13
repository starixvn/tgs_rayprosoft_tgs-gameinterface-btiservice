namespace Tgs.GameInterface.BTiService.Core.ApplicationServices;

public record GameRecordQuery(
    Uri ApiUrl,
    string AgentUserName,
    string AgentPassword,
    DateTimeOffset StartTime,
    DateTimeOffset EndTime,
    int Page = 0,
    int PageLimit = 1000);
