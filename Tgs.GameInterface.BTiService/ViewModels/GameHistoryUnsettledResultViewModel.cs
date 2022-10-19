namespace Tgs.GameInterface.BTiService.ViewModels;

public record GameHistoryUnsettledResultViewModel(
    string PlayerCode,
    string ExternalRoundId,
    DateTimeOffset ExternalCreatedOn,
    decimal Bet,
    decimal ValidBet,
    string AdditionalData);
