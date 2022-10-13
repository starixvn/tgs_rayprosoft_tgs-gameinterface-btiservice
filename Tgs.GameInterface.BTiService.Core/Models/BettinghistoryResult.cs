namespace Tgs.GameInterface.BTiService.Core.Models;

public record BettinghistoryResult(
    bool Success,
    BettingHistoryViewModel? Transcations = null);
