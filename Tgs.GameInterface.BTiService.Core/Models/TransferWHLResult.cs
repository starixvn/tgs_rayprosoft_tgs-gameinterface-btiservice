namespace Tgs.GameInterface.BTiService.Core.Models;

public record TransferWHLResult(
    string RefTransactionCode,
    string TransactionId,
    decimal Balance);
