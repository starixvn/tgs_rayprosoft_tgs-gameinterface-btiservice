namespace Tgs.GameInterface.BTiService.Core.ApplicationServices;

public record FundInResult(
    bool Success,
    string? TransactionId,
    decimal? BalanceAfter = null);
