using System.Net;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.BTiService.Core.UnitTest;

[TestClass]
public class BettingHistoryUnitTest
{
    [TestMethod]
    public async Task Normal_Process()
    {
        var apiUrl = new Uri("https://get-history-bets-d5nlg5buca-de.a.run.app/");
        var token = "zZ1ZVyOZ2HTHC-4GUOodoXazpv8";
        var startTime = DateTime.UtcNow.AddMinutes(-5);
        var endTime = DateTime.UtcNow;

        var response = "{\"errorCode\":0,\"LastUpdateDate\":\"2020-04-28T02:48:36.307\",\"Bets\":[{\"PL\":-5.0,\"NonCashOutAmount\":0.0,\"ComboBonusAmount\":0.0,\"BetSettledDate\":\"2020-04-10T01:12:24.097\",\"PurchaseID\":\"637220813905354889\",\"UpdateDate\":\"2020-04-10T02:12:24.097\",\"Odds\":365,\"OddsInUserStyle\":\"4.65\",\"OddsStyleOfUser\":\"European\",\"TotalStake\":5.0,\"OddsDec\":4.65,\"ValidStake\":5.0,\"Platform\":\"Web\",\"Return\":0.0,\"DomainID\":379,\"FreeBet\":{\"Amount\":20.0,\"IsRiskFreeBet\":0},\"RealMoneyAmount\":30.0,\"BetStatus\":\"Lost\",\"Brand\":\"BTIAsiaSL\",\"UserName\":\"bti_leo_cn\",\"BetTypeName\":\"Single bets\",\"BetTypeId\":1,\"CreationDate\":\"2020-04-10T02:09:50.933\",\"Status\":\"Lost\",\"CustomerID\":23164222,\"MerchantCustomerID\":\"bti_leo_cn\",\"Currency\":\"CNY\",\"PlayerLevelID\":10,\"PlayerLevelName\":\"Test User\",\"SearchDateTime\":\"2021-08-17T02:32:45.000\",\"Selections\":[{\"isresettled\":0,\"RelatedBetID\":0,\"ActionType\":\"Real\",\"BonusID\":0,\"CouponCode\":\"\",\"ReferenceID\":0,\"BetID\":\"637220813905354890\",\"LineID\":636549245,\"Odds\":365,\"OddsInUserStyle\":\"4.65\",\"LeagueName\":\"Friendly\",\"LeagueID\":921545223232323291,\"HomeTeam\":\"Landskrona BoIS\",\"AwayTeam\":\"Eskilsminne IF\",\"BranchName\":\"Soccer\",\"BranchID\":1,\"LineTypeName\":\"1X2\",\"Points\":0.0,\"Score\":\"\",\"YourBet\":\" Eskilsminne IF\",\"EventDate\":\"2020-04-10T11:00:00\",\"EventTypeName\":\"FT\",\"BetType\":\"Single bets\",\"isfreebet\":0,\"OddsDec\":4.65,\"LiveScore1\":0,\"LiveScore2\":0,\"Status\":\"Lost\",\"GameID\":17700454545454505,\"EventTypeID\":3453454360,\"LineTypeID\":3454354354351,\"IsLive\":0,\"UpdateDate\":\"2020-04-10T12:40:42.817\",\"IsNewLine\":0}]}]}";
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
            .BettingHistoryAsync(
                apiUrl: apiUrl,
                token: token,
                startTime: startTime,
                endTime: endTime)
            .ToArrayAsync()
            .ConfigureAwait(false);

        Assert.IsNotNull(actual);
        Assert.IsTrue(actual.Length > 0);
    }
}
