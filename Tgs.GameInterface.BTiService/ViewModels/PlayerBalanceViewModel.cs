namespace Tgs.GameInterface.BTiService.ViewModels;

public record PlayerBalanceViewModel(
    Uri ApiUrl,
    string AgentUserName,
    string AgentPassword,
    string MerchantCustomerCode);
