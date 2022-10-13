namespace Tgs.GameInterface.BTiService.Core.Models;

public record GetCustomerByMerchantCodeResult(
    bool Success,
    int? CustomerID = null,
    decimal? Balance = null);
