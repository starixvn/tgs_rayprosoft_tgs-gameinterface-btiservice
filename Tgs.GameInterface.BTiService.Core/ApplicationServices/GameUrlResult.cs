namespace Tgs.GameInterface.BTiService.Core.ApplicationServices;

public record GameUrlResult(
    bool Success,
    string? Message = null,
    bool IsPlayerExist = false,
    string? GameUrl = null);
