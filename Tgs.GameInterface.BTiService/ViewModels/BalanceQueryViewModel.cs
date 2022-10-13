namespace Tgs.GameInterface.BTiService.ViewModels;

public record BalanceQueryViewModel(
    Uri ApiUrl,
    string AgentUserName,
    string AgentPassword,
    string PlayerCode);
