namespace Tgs.GameInterface.BTiService.Core.ApplicationServices;

public record GameWagerSettledQuery(
    Uri DataApiUrl,
    string AgentUserName,
    string AgentPassword,
    DateTimeOffset StartTime,
    DateTimeOffset EndTime,
    int Page = 0,
    int PageLimit = 1000);
