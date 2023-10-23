namespace Tgs.GameInterface.BTiService.Core.ApplicationServices;

public record FundResult(
    string RefTransactionCode,
    string TransactionId,
    decimal Balance,
    string Resp);
