namespace Tgs.GameInterface.BTiService.Core.ApplicationServices;

public record BalanceResult(
    bool Success,
    bool IsPlayerExist,
    decimal? Balance);
