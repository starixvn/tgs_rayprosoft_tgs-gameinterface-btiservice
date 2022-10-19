namespace Tgs.GameInterface.BTiService.Core.ApplicationServices;

public record PlayerCreateCommand(
    Uri ApiUrl,
    string AgentUserName,
    string AgentPassword,
    string Brand,
    string MerchantCustomerCode,
    string Currency);
