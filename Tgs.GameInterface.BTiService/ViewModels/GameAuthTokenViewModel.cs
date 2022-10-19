namespace Tgs.GameInterface.BTiService.ViewModels;

public record GameAuthTokenViewModel(
    Uri ApiUrl,
    string AgentUserName,
    string AgentPassword,
    string MerchantCustomerCode);
