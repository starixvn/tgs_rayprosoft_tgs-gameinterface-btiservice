namespace Tgs.GameInterface.BTiService.Core.Models;

public record TransferFromWHLResult(
    bool Success,
    int? TransactionId = null,
    decimal? BalanceAfter = null);
