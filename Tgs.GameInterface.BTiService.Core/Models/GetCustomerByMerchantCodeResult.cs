namespace Tgs.GameInterface.BTiService.Core.Models;

public record GetCustomerByMerchantCodeResult(
    bool IsPlayerExist,
    decimal Balance);
