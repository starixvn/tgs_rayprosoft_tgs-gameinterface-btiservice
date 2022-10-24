namespace Tgs.GameInterface.BTiService.Core.Models;

public record OpenBet
{
    public DateTime BetSettledDate { get; set; }
    public string BetStatus { get; set; } = default!;
    public int BetTypeId { get; set; }
    public string BetTypeName { get; set; } = default!;
    public string Brand { get; set; } = default!;
    public decimal ComboBonusAmount { get; set; }
    public DateTime CreationDate { get; set; }
    public string Currency { get; set; } = default!;
    public int CustomerID { get; set; }
    public int DomainID { get; set; }
    public OpenBetFreeBet FreeBet { get; set; } = default!;
    public string Gain { get; set; } = default!;
    public string MerchantCustomerID { get; set; } = default!;
    public decimal NonCashOutAmount { get; set; }
    public int Odds { get; set; }
    public decimal OddsDec { get; set; }
    public string OddsInUserStyle { get; set; } = default!;
    public string OddsStyleOfUser { get; set; } = default!;
    public decimal PL { get; set; }
    public string Platform { get; set; } = default!;
    public int PlayerLevelID { get; set; }
    public string PlayerLevelName { get; set; } = default!;
    public string PurchaseID { get; set; } = default!;
    public decimal RealMoneyAmount { get; set; }
    public decimal Return { get; set; }
    public DateTime SearchDateTime { get; set; }
    public IEnumerable<OpenBetSelection> Selections { get; set; } = default!;
    public string SystemName { get; set; } = default!;
    public decimal TotalStake { get; set; }
    public DateTime UpdateDate { get; set; }
    public string UserName { get; set; } = default!;
    public decimal ValidStake { get; set; }
}






