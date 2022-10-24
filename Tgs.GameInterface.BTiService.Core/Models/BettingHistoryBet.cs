namespace Tgs.GameInterface.BTiService.Core.Models;

public record BettingHistoryBet
{
    public decimal PL { get; set; }
    public decimal NonCashOutAmount { get; set; }
    public decimal ComboBonusAmount { get; set; }
    public DateTime BetSettledDate { get; set; }
    public string PurchaseID { get; set; } = default!;
    public DateTime? UpdateDate { get; set; }
    public int Odds { get; set; }
    public string OddsInUserStyle { get; set; } = default!;
    public string OddsStyleOfUser { get; set; } = default!;
    public decimal TotalStake { get; set; }
    public decimal OddsDec { get; set; } = default!;
    public decimal ValidStake { get; set; }
    public string Platform { get; set; } = default!;
    public decimal Return { get; set; }
    public int DomainID { get; set; }
    public BettingHistoryBetFreeBet FreeBet { get; set; } = default!;
    public decimal RealMoneyAmount { get; set; }
    public string BetStatus { get; set; } = default!;
    public string Brand { get; set; } = default!;
    public string UserName { get; set; } = default!;
    public string BetTypeName { get; set; } = default!;
    public int BetTypeId { get; set; }
    public DateTime CreationDate { get; set; }
    public int CustomerID { get; set; }
    public string MerchantCustomerID { get; set; } = default!;
    public string Currency { get; set; } = default!;
    public int PlayerLevelID { get; set; }
    public string PlayerLevelName { get; set; } = default!;
    public DateTime SearchDateTime { get; set; }
    public IEnumerable<BettingHistoryBetSelection> Selections { get; set; } = default!;
    public string AdditionalData { get; set; } = default!;
}
