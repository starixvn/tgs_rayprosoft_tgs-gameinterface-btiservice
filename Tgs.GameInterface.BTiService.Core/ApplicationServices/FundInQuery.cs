namespace Tgs.GameInterface.BTiService.Core.ApplicationServices;

public record FundInQuery(
    Uri ApiUrl,
    string AgentUserName,
    string AgentPassword,
    string PlayerCode,
    string TransactionId,
    decimal Amount);
