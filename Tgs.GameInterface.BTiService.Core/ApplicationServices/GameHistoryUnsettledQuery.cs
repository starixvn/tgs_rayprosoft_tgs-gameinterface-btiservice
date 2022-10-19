namespace Tgs.GameInterface.BTiService.Core.ApplicationServices;

public record GameHistoryUnsettledQuery(
    Uri TokenApiUrl,
    Uri OpenBetsApiUrl,
    string AgentUserName,
    string AgentPassword,
    DateTimeOffset StartTime,
    DateTimeOffset EndTime);
