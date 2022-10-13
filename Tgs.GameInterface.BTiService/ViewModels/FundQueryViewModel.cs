namespace Tgs.GameInterface.BTiService.ViewModels;

public record FundQueryViewModel(
    Uri ApiUrl,
    string AgentUserName,
    string AgentPassword,
    string PlayerCode,
    string TransactionId,
    decimal Amount);
