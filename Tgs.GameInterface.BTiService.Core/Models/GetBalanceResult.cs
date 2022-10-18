namespace Tgs.GameInterface.BTiService.Core.Models;

public record GetBalanceResult(
    bool IsPlayerExist,
    decimal Balance);
