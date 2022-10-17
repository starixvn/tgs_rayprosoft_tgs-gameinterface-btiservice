namespace Tgs.GameInterface.BTiService.Core.ApplicationServices;

public record BalanceResult(
    bool IsPlayerExist,
    decimal? Balance);
