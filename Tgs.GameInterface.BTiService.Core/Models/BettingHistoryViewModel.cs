namespace Tgs.GameInterface.BTiService.Core.Models;

public class Bet
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

    public Freebet FreeBet { get; set; } = default!;

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

    public Selection[] Selections { get; set; } = default!;

    public string Status { get; set; } = default!;

    public decimal TotalStake { get; set; }

    public DateTime UpdateDate { get; set; }

    public string UserName { get; set; } = default!;

    public decimal ValidStake { get; set; }
}

public class BettingHistoryViewModel
{
    public Bet[] Bets { get; set; } = default!;

    public int errorCode { get; set; }

    public DateTime LastUpdateDate { get; set; }
}

public class Freebet
{
    public decimal Amount { get; set; }

    public int IsRiskFreeBet { get; set; }
}

public class Selection
{
    public string ActionType { get; set; } = default!;

    public string AwayTeam { get; set; } = default!;

    public string BetID { get; set; } = default!;

    public string BetType { get; set; } = default!;

    public int BonusID { get; set; }

    public int BranchID { get; set; }

    public string BranchName { get; set; } = default!;

    public string CouponCode { get; set; } = default!;

    public DateTime EventDate { get; set; }

    public long EventTypeID { get; set; }

    public string EventTypeName { get; set; } = default!;

    public long GameID { get; set; }

    public string HomeTeam { get; set; } = default!;

    public int isfreebet { get; set; }

    public int IsLive { get; set; }

    public int IsNewLine { get; set; }

    public int isresettled { get; set; }

    public long LeagueID { get; set; }

    public string LeagueName { get; set; } = default!;

    public int LineID { get; set; }

    public long LineTypeID { get; set; }

    public string LineTypeName { get; set; } = default!;

    public int LiveScore1 { get; set; }

    public int LiveScore2 { get; set; }

    public int Odds { get; set; }

    public decimal OddsDec { get; set; }

    public string OddsInUserStyle { get; set; } = default!;

    public decimal Points { get; set; }

    public int ReferenceID { get; set; }

    public int RelatedBetID { get; set; }

    public string Score { get; set; } = default!;

    public string Status { get; set; } = default!;

    public DateTime UpdateDate { get; set; }

    public string YourBet { get; set; } = default!;
}
