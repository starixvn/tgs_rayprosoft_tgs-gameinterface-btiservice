namespace Tgs.GameInterface.BTiService.Core.Models;

public record CheckTransactionResult(
    bool Success,
    int? TransactionID = null,
    decimal? BalanceAfter = null);
