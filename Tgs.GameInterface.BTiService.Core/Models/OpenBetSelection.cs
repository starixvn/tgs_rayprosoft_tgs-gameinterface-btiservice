namespace Tgs.GameInterface.BTiService.Core.Models;

public record OpenBetSelection
{
    public string ActionType { get; set; } = default!;
    public string AwayTeam { get; set; } = default!;
    public string BetID { get; set; } = default!;
    public string BetType { get; set; } = default!;
    public int BonusID { get; set; }
    public int BranchID { get; set; }
    public string BranchName { get; set; } = default!;
    public string CouponCode { get; set; } = default!;
    public string EncodedLineID { get; set; } = default!;
    public DateTime EventDate { get; set; }
    public long EventTypeID { get; set; }
    public string EventTypeName { get; set; } = default!;
    public long GameID { get; set; }
    public string HomeTeam { get; set; } = default!;
    public int IsLive { get; set; }
    public int IsNewLine { get; set; }
    public long LeagueID { get; set; }
    public string LeagueName { get; set; } = default!;
    public long LineID { get; set; }
    public long LineTypeID { get; set; }
    public string LineTypeName { get; set; } = default!;
    public int LiveScore1 { get; set; }
    public int LiveScore2 { get; set; }
    public int Odds { get; set; }
    public decimal OddsDec { get; set; }
    public string OddsInUserStyle { get; set; } = default!;
    public decimal Points { get; set; }
    public int ReferenceID { get; set; }
    public long RelatedBetID { get; set; }
    public string Score { get; set; } = default!;
    public string Status { get; set; } = default!;
    public DateTime? UpdateDate { get; set; } = default!;
    public string YourBet { get; set; } = default!;
    public int Isfreebet { get; set; }
}
