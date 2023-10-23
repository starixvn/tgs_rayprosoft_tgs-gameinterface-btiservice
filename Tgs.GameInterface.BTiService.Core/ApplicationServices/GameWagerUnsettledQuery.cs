namespace Tgs.GameInterface.BTiService.Core.ApplicationServices;

public record GameWagerUnsettledQuery(
    Uri DataApiUrl,
    string AgentUserName,
    string AgentPassword,
    DateTimeOffset StartTime,
    DateTimeOffset EndTime);
