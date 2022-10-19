namespace Tgs.GameInterface.BTiService.ViewModels;

public record PlayerCreateViewModel(
    Uri ApiUrl,
    string AgentUserName,
    string AgentPassword,
    string Brand,
    string MerchantCustomerCode,
    string Currency);
