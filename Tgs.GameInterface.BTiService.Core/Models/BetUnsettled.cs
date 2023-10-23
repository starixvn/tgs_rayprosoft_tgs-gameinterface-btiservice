namespace Tgs.GameInterface.BTiService.Core.Models;

public record BetUnsettled(
    string MerchantCustomerID,
    string PurchaseID,
    string BetStatus,
    DateTimeOffset CreationDate,
    decimal TotalStake,
    decimal ValidStake,
    string AdditionalData);
