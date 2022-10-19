namespace Tgs.GameInterface.BTiService.Core.ApplicationServices;

public record GameGetAuthTokenQuery(
    Uri ApiUrl,
    string AgentUserName,
    string AgentPassword,
    string MerchantCustomerCode);
