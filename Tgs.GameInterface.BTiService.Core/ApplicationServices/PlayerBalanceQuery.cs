namespace Tgs.GameInterface.BTiService.Core.ApplicationServices;

public record PlayerBalanceQuery(
    Uri ApiUrl,
    string AgentUserName,
    string AgentPassword,
    string MerchantCustomerCode);
