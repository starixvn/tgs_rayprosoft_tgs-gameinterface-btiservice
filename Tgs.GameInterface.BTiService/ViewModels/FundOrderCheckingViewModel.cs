namespace Tgs.GameInterface.BTiService.ViewModels;

public record FundOrderCheckingViewModel(
    Uri ApiUrl,
    string AgentUserName,
    string AgentPassword,
    string TransactionId);
