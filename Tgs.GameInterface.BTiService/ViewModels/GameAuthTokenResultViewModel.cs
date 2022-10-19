namespace Tgs.GameInterface.BTiService.ViewModels;

public record GameAuthTokenResultViewModel(
    bool IsPlayerExist,
    string AuthToken);
