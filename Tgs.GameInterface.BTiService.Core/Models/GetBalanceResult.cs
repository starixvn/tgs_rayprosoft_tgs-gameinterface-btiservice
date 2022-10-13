namespace Tgs.GameInterface.BTiService.Core.Models;

public record GetBalanceResult(
    bool Success,
    bool IsPlayerExist = false,
    decimal? Balance = null);
