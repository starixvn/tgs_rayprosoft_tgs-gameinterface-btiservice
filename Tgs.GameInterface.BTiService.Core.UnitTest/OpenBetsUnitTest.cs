using System.Net;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.BTiService.Core.UnitTest;

[TestClass]
public class OpenBetsUnitTest
{
    [TestMethod]
    public async Task Normal_Process()
    {
        var apiUrl = new Uri("https://get-open-bets-d5nlg5buca-de.a.run.app/");
        var token = "zZ1ZVyOZ2HTHC-4GUOodoXazpv8";
        var startTime = DateTime.UtcNow.AddMinutes(-5);
        var endTime = DateTime.UtcNow;

        var response = "{\"errorCode\":0,\"LastUpdateDate\":\"2020-04-28T03:11:32.637\",\"Bets\":[{\"Gain\":18.06,\"PurchaseID\":\"637236402640144444\",\"UpdateDate\":\"2020-04-28T03:11:03.587\",\"Odds\":-124,\"OddsInUserStyle\":\"0.8\",\"OddsStyleOfUser\":\"Hongkong\",\"TotalStake\":10.0,\"OddsDec\":1.81,\"ValidStake\":0.0,\"Platform\":\"Web\",\"Return\":0.0,\"DomainID\":379,\"FreeBet\":{\"Amount\":20.0,\"IsRiskFreeBet\":0},\"BetStatus\":\"Opened\",\"Brand\":\"BTIAsiaSL\",\"UserName\":\"bti_leo_cn\",\"BetTypeName\":\"Single bets\",\"BetTypeId\":1,\"CreationDate\":\"2020-04-28T03:11:03.587\",\"Status\":\"Opened\",\"CustomerID\":23164222,\"MerchantCustomerID\":\"bti_leo_cn\",\"Currency\":\"CNY\",\"PlayerLevelID\":10,\"PlayerLevelName\":\"Test User\",\"SearchDateTime\":\"2021-08-17T02:32:45.000\",\"Selections\":[{\"BetID\":\"637236402640144445\",\"LineID\":642321517,\"Odds\":-124,\"OddsInUserStyle\":\"0.8\",\"LeagueName\":\"Poland - Ekstraklasa SRL\",\"LeagueID\":2007234432318,\"HomeTeam\":\"Wisla Krakow SRL\",\"AwayTeam\":\"Legia Warsaw SRL\",\"BranchName\":\"Soccer\",\"BranchID\":1,\"LineTypeName\":\"OU\",\"Points\":2.5,\"Score\":\"\",\"YourBet\":\" Over : 2.5\",\"EventDate\":\"2020-04-28T18:30:00\",\"EventTypeName\":\"FT\",\"BetType\":\"Single bets\",\"isfreebet\":0,\"OddsDec\":1.81,\"LiveScore1\":0,\"LiveScore2\":0,\"Status\":\"Opened\",\"GameID\":1788594354353411,\"EventTypeID\":0,\"LineTypeID\":3,\"IsLive\":0,\"IsNewLine\":0}]}]}";
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
            .OpenBetsAsync(
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
