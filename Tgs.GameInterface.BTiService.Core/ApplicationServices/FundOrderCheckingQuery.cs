namespace Tgs.GameInterface.BTiService.Core.ApplicationServices;

public record FundOrderCheckingQuery(
    Uri ApiUrl,
    string AgentUserName,
    string AgentPassword,
    string TransactionId);
