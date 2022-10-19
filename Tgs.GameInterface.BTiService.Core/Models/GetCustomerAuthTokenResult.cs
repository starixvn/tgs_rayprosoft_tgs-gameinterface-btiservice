namespace Tgs.GameInterface.BTiService.Core.Models;

public record GetCustomerAuthTokenResult(
    bool IsPlayerExist,
    string AuthToken);
