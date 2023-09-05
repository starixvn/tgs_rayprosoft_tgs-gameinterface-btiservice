using System.Net;
using System.Web;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.BTiService.Core.UnitTest;

[TestClass]
public class BettingHistoryPaginationUnitTest
{
    [TestMethod]
    public async Task Normal_Process()
    {
        var apiUrl = new Uri("https://get-history-bets-paging-d5nlg5buca-de.a.run.app/");
        var token = "zZ1ZVyOZ2HTHC-4GUOodoXazpv8";
        var startTime = DateTime.UtcNow.AddMinutes(-5);
        var endTime = DateTime.UtcNow;
        var page = 0;
        var pageLimit = 100;

        var response = "{\"Bets\":[{\"BetSettledDate\":\"2023-09-05T03:12:02.692\",\"BetStatus\":\"Won\",\"BetTypeId\":1,\"BetTypeName\":\"Single bets\",\"Brand\":\"TGSPB1\",\"ComboBonusAmount\":0.0,\"CreationDate\":\"2023-09-05T02:40:58.198\",\"Currency\":\"PHP\",\"CustomerID\":120728610,\"DomainID\":0,\"FreeBet\":{\"Amount\":0.0,\"IsRiskFreeBet\":0},\"MerchantCustomerID\":\"4uk3ac31cpq2\",\"NonCashOutAmount\":0.0,\"NumberOfBets\":1,\"Odds\":209,\"OddsDec\":3.09,\"OddsInUserStyle\":\"3.09\",\"OddsStyleOfUser\":\"European\",\"PL\":236.17,\"Platform\":\"Mobile\",\"PlayerLevelID\":0,\"PlayerLevelName\":\"Newly Registered\",\"PurchaseID\":\"486727412961468416\",\"RealMoneyAmount\":113.0,\"Return\":349.17,\"SearchDateTime\":\"2023-09-05T03:16:28.229\",\"Selections\":[{\"ActionType\":\"Real\",\"AwayTeam\":\"Isidro Metapan\",\"BetID\":\"486727412961468417\",\"BetType\":\"Single bets\",\"BonusID\":0,\"BranchID\":2,\"BranchName\":\"Basketball\",\"CouponCode\":\"\",\"EncodedLineID\":\"0ML486270799121313793H\",\"EventDate\":\"2023-09-05T01:15:00\",\"EventTypeID\":39,\"EventTypeName\":\"Live Betting Moneyline\",\"GameID\":486270798265675776,\"HomeTeam\":\"Santa Tecla\",\"IsEarlyPayout\":0,\"IsLive\":1,\"IsNewLine\":0,\"LeagueID\":484262326766497792,\"LeagueName\":\"El Salvador - Liga Mayor\",\"LineID\":0,\"LineTypeID\":1,\"LineTypeName\":\"Moneyline\",\"LiveScore1\":52,\"LiveScore2\":53,\"Odds\":209,\"OddsDec\":3.09,\"OddsInUserStyle\":\"3.09\",\"Points\":0.0,\"ReferenceID\":0,\"RelatedBetID\":0,\"Score\":\"70 : 66\",\"Status\":\"Won\",\"UpdateDate\":null,\"YourBet\":\"Santa Tecla\",\"isfreebet\":0,\"isresettled\":0}],\"Status\":\"Won\",\"SystemName\":\"0/1\",\"TotalStake\":113.0,\"UpdateDate\":\"2023-09-05T03:12:02.692\",\"UserName\":\"4uk3ac31cpq2\",\"ValidStake\":113.0}],\"LastUpdateDate\":\"2023-09-05T08:12:42.772000Z\",\"currentPage\":0,\"errorCode\":0,\"totalPages\":0}";

        var uriBuilder = new UriBuilder(apiUrl);
        var query = HttpUtility.ParseQueryString(uriBuilder.Query);
        query["token"] = token;
        uriBuilder.Query = query.ToString();
        var handler = new MockHttpMessageHandler();
        _ = handler
            .Expect(HttpMethod.Post, uriBuilder.Uri.ToString())
            .Respond(HttpStatusCode.OK, new StringContent(response));

        var fakeHttpClient = handler.ToHttpClient();
        var fakeLog = NullLogger<GameDataApiClient>.Instance;

        var sut = new GameDataApiClient(fakeHttpClient, fakeLog);

        var actual = await sut
            .BettingHistoryPaginationAsync(
                apiUrl: apiUrl,
                token: token,
                startTime: startTime,
                endTime: endTime,
                page: page,
                pageLimit: pageLimit)
            .ToArrayAsync()
            .ConfigureAwait(false);

        Assert.IsNotNull(actual);
        Assert.IsTrue(actual.Length > 0);
    }
}
