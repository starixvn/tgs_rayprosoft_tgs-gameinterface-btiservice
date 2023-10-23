namespace Tgs.GameInterface.BTiService.ViewModels;

public record GameWagerResultViewModel(
    string PlayerCode,
    string ExternalWagerId,
    bool IsClosed,
    string Status,
    DateTimeOffset ExternalCreatedOn,
    DateTimeOffset ExternalClosedOn,
    decimal Bet,
    decimal ValidBet,
    decimal Settle,
    decimal WinLoss,
    string AdditionalData);
