using Tgs.GameInterface.BTiService.Protos;

namespace Tgs.GameInterface.BTiService.ViewModels;

public record GameHistorySettledResultViewModel(
    string PlayerCode,
    string ExternalRoundId,
    bool IsClosed,
    GameHistoryStatus Status,
    DateTimeOffset ExternalCreatedOn,
    DateTimeOffset ExternalClosedOn,
    decimal Bet,
    decimal ValidBet,
    decimal PlayerWinLoss,
    decimal Turnover,
    decimal Settle,
    string AdditionalData);
