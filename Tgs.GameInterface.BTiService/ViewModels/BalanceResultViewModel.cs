namespace Tgs.GameInterface.BTiService.ViewModels;

public record BalanceResultViewModel(
    bool Success,
    bool IsPlayerExist,
    decimal? Balance);
