namespace Tgs.GameInterface.BTiService.Core.ApplicationServices;

public record GameWagerResult(
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
