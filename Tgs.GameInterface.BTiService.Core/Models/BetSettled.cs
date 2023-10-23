namespace Tgs.GameInterface.BTiService.Core.Models;

public record BetSettled(
    string MerchantCustomerID,
    string PurchaseID,
    string BetStatus,
    DateTimeOffset CreationDate,
    DateTimeOffset BetSettledDate,
    decimal TotalStake,
    decimal ValidStake,
    decimal PL,
    decimal Return,
    string AdditionalData);
