namespace Tgs.GameInterface.BTiService.Core.ApplicationServices;

public record PlayerCreateCommand(
    Uri ApiUrl,
    string AgentUserName,
    string AgentPassword,
    string BrandCode,
    string MerchantCustomerCode,
    string Currency);
