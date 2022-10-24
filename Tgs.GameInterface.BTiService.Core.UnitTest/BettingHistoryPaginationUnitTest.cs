using System.Net;
using Microsoft.AspNetCore.WebUtilities;
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

        var response = "{\"Bets\":[{\"BetSettledDate\":\"2022-10-24T04:07:27.832\",\"BetStatus\":\"Won\",\"BetTypeId\":1,\"BetTypeName\":\"Single bets\",\"Brand\":\"GPIRMB\",\"ComboBonusAmount\":0.0,\"CreationDate\":\"2022-10-24T03:36:26.853\",\"Currency\":\"CNY\",\"CustomerID\":108901496,\"DomainID\":0,\"FreeBet\":{\"Amount\":0.0,\"IsRiskFreeBet\":0},\"MerchantCustomerID\":\"4ued4qr92gm0\",\"NonCashOutAmount\":0,\"Odds\":-1000,\"OddsDec\":1.1,\"OddsInUserStyle\":\"1.1\",\"OddsStyleOfUser\":\"European\",\"PL\":0.1,\"Platform\":\"Web\",\"PlayerLevelID\":10,\"PlayerLevelName\":\"Test User\",\"PurchaseID\":\"372226808545976320\",\"RealMoneyAmount\":1.0,\"Return\":1.1,\"SearchDateTime\":\"2022-10-24T04:10:07.749\",\"Selections\":[{\"ActionType\":\"Real\",\"AwayTeam\":\"Oakland Roots\",\"BetID\":\"372226808545976321\",\"BetType\":\"Single bets\",\"BonusID\":0,\"BranchID\":1,\"BranchName\":\"Soccer\",\"CouponCode\":\"\",\"EncodedLineID\":\"0ML371623360972742656A\",\"EventDate\":\"2022-10-24T02:00:00\",\"EventTypeID\":39,\"EventTypeName\":\"Live Betting 1X2\",\"GameID\":371623359915864064,\"HomeTeam\":\"San Diego Loyal SC\",\"IsLive\":1,\"IsNewLine\":0,\"LeagueID\":288588660704821248,\"LeagueName\":\"USA - USL Championship\",\"LineID\":0,\"LineTypeID\":1,\"LineTypeName\":\"1X2\",\"LiveScore1\":0,\"LiveScore2\":2,\"Odds\":-1000,\"OddsDec\":1.1,\"OddsInUserStyle\":\"1.1\",\"Points\":0.0,\"ReferenceID\":0,\"RelatedBetID\":0,\"Score\":\"0 : 3\",\"Status\":\"Won\",\"UpdateDate\":null,\"YourBet\":\"Oakland Roots\",\"isfreebet\":0,\"isresettled\":0}],\"Status\":\"Won\",\"SystemName\":\"0/1\",\"TotalStake\":1.0,\"UpdateDate\":\"2022-10-24T04:07:27.832\",\"UserName\":\"4ued4qr92gm0\",\"ValidStake\":1.0}],\"LastUpdateDate\":\"2022-10-24T07:49:20.828000Z\",\"currentPage\":0,\"errorCode\":0,\"totalPages\":5}";
        var query = new Dictionary<string, string>
        {
            {"token", token}
        };
        var url = QueryHelpers.AddQueryString(apiUrl.ToString(), query);
        var handler = new MockHttpMessageHandler();
        _ = handler.Expect(HttpMethod.Post, url)
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
