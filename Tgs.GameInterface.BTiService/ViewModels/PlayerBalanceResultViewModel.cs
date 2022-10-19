namespace Tgs.GameInterface.BTiService.ViewModels;

public record PlayerBalanceResultViewModel(
    bool IsPlayerExist,
    decimal? Balance);
