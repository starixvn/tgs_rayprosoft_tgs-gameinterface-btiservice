namespace Tgs.GameInterface.BTiService.Core.ApplicationServices;

public record GameAuthTokenQuery(
    Uri ApiUrl,
    string AgentUserName,
    string AgentPassword,
    string MerchantCustomerCode);
