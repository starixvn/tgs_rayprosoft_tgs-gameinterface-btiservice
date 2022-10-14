namespace Tgs.GameInterface.BTiService.ViewModels;

public record FundResultViewModel(
    bool Success,
    string? TransactionId,
    decimal? BalanceAfter = null);
