namespace Tgs.GameInterface.BTiService.ViewModels;

public record FundStatusViewModel(
    Uri ApiUrl,
    string AgentUserName,
    string AgentPassword,
    string RefTransactionCode);
