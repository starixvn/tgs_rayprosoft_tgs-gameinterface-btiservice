namespace Tgs.GameInterface.BTiService.Core.ApplicationServices;

public record GameHistoryUnsettledResult(
    string PlayerCode,
    string ExternalRoundId,
    DateTimeOffset ExternalCreatedOn,
    decimal Bet,
    decimal ValidBet,
    string AdditionalData);
