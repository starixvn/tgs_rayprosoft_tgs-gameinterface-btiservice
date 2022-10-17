namespace Tgs.GameInterface.BTiService.Core.Models;

public record GetBalanceResult
{
    public bool IsPlayerExist { get; init; }
    public decimal Balance { get; init; }
}
