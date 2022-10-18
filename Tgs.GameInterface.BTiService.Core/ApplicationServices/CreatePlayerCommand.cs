namespace Tgs.GameInterface.BTiService.Core.ApplicationServices;

public record CreatePlayerCommand(
    Uri ApiUrl,
    string AgentUserName,
    string AgentPassword,
    string Brand,
    string PlayerCode,
    string Currency);
