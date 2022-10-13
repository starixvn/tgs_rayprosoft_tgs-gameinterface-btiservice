namespace Tgs.GameInterface.BTiService.Core.Models;

public record TransferToWHLResult(
    bool Success,
    int? TransactionId = null,
    decimal? BalanceAfter = null);
