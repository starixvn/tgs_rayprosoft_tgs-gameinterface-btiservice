namespace Tgs.GameInterface.BTiService.ViewModels;

public record FundViewModel(
    Uri ApiUrl,
    string AgentUserName,
    string AgentPassword,
    string MerchantCustomerCode,
    string RefTransactionCode,
    decimal Amount);
