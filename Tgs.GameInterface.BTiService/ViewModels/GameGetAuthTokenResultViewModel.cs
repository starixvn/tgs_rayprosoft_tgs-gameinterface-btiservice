namespace Tgs.GameInterface.BTiService.ViewModels;

public record GameGetAuthTokenResultViewModel(
    bool IsPlayerExist,
    string AuthToken);
