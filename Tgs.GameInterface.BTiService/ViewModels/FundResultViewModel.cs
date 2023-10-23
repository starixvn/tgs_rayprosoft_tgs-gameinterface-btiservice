namespace Tgs.GameInterface.BTiService.ViewModels;

public record FundResultViewModel(
    string RefTransactionCode,
    string TransactionId,
    decimal Balance,
    string Resp);
