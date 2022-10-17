using System.Text.Json.Serialization;
using Tgs.GameInterface.BTiService.Core.JsonConverters;

namespace Tgs.GameInterface.BTiService.Core.Models;

public class BettingHistoryBetSelection
{
    public int RelatedBetID { get; set; }

    public string ActionType { get; set; } = default!;

    public int BonusID { get; set; }

    public string CouponCode { get; set; } = default!;

    public int ReferenceID { get; set; }

    public string BetID { get; set; } = default!;

    public int LineID { get; set; }

    public int Odds { get; set; }

    public string OddsInUserStyle { get; set; } = default!;

    public string LeagueName { get; set; } = default!;

    public long LeagueID { get; set; }

    public string HomeTeam { get; set; } = default!;

    public string AwayTeam { get; set; } = default!;

    public string BranchName { get; set; } = default!;

    public int BranchID { get; set; }

    public string LineTypeName { get; set; } = default!;

    public float Points { get; set; }

    public string Score { get; set; } = default!;

    public string YourBet { get; set; } = default!;

    public DateTime EventDate { get; set; }

    public string EventTypeName { get; set; } = default!;

    public string BetType { get; set; } = default!;

    public int Isfreebet { get; set; }

    public decimal OddsDec { get; set; }

    [JsonConverter(typeof(JsonNumberToStringConverter))]
    public string LiveScore1 { get; set; } = default!;

    [JsonConverter(typeof(JsonNumberToStringConverter))]
    public string LiveScore2 { get; set; } = default!;

    public string Status { get; set; } = default!;

    public long GameID { get; set; }

    public long EventTypeID { get; set; }

    public long LineTypeID { get; set; }

    public int IsLive { get; set; }

    public DateTime UpdateDate { get; set; }

    public int IsNewLine { get; set; }
}
