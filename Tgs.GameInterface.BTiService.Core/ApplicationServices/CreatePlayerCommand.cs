namespace Tgs.GameInterface.BTiService.Core.ApplicationServices;

public record CreatePlayerCommand(
    Uri ApiUrl,
    string AgentUserName,
    string AgentPassword,
    string PlayerCode,
    string Currency);
