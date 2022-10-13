namespace Tgs.GameInterface.BTiService.Core.ApplicationServices;

public record BalanceQuery(
    Uri ApiUrl,
    string AgentUserName,
    string AgentPassword,
    string PlayerCode);
