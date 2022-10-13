namespace Tgs.GameInterface.BTiService.Core.ApplicationServices;

public record FundOutQuery(
    Uri ApiUrl,
    string AgentUserName,
    string AgentPassword,
    string PlayerCode,
    string TransactionId,
    decimal Amount);
