namespace Tgs.GameInterface.BTiService.Core.ApplicationServices;

public record FundStatusQuery(
    Uri ApiUrl,
    string AgentUserName,
    string AgentPassword,
    string RefTransactionCode);
