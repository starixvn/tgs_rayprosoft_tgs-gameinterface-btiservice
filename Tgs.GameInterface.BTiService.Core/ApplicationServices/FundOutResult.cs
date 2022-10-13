namespace Tgs.GameInterface.BTiService.Core.ApplicationServices;

public record FundOutResult(
    bool Success,
    string? TransactionId,
    decimal? BalanceAfter = null);
