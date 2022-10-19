namespace Tgs.GameInterface.BTiService.Core.ApplicationServices;

public record PlayerBalanceResult(
    bool IsPlayerExist,
    decimal? Balance);
