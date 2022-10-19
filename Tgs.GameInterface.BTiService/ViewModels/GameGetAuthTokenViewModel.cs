namespace Tgs.GameInterface.BTiService.ViewModels;

public record GameGetAuthTokenViewModel(
    Uri ApiUrl,
    string AgentUserName,
    string AgentPassword,
    string MerchantCustomerCode);
