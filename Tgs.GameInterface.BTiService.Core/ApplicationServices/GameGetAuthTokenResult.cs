namespace Tgs.GameInterface.BTiService.Core.ApplicationServices;

public record GameGetAuthTokenResult(
    bool IsPlayerExist,
    string AuthToken);
