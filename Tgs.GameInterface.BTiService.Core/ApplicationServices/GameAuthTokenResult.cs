namespace Tgs.GameInterface.BTiService.Core.ApplicationServices;

public record GameAuthTokenResult(
    bool IsPlayerExist,
    string AuthToken);
