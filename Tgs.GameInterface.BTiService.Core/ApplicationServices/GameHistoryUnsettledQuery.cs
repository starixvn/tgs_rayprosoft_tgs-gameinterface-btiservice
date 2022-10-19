namespace Tgs.GameInterface.BTiService.Core.ApplicationServices;

public record GameHistoryUnsettledQuery(
    Uri TokenApiUrl,
    Uri OpenBetsApiUrl,
    string AgentUserName,
    string AgentPassword,
    DateTime StartTime,
    DateTime EndTime);
