namespace Tgs.GameInterface.BTiService.ViewModels;

public record CreatePlayerViewModel(
    Uri ApiUrl,
    string AgentUserName,
    string AgentPassword,
    string Brand,
    string PlayerCode,
    string Currency);
