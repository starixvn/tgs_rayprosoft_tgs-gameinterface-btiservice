using System.Net;
using System.Text.Json.Nodes;
using System.Web;
using Microsoft.Extensions.Logging.Abstractions;
using RichardSzalay.MockHttp;
using Tgs.GameInterface.BTiService.Core.Models;

namespace Tgs.GameInterface.BTiService.Core.UnitTest;

public class GetOpenBetsPagingUnitTest
{
    [Fact]
    public async Task Normal_Process()
    {
        var apiUrl = new Uri("https://data-api.442hattrick.com/");
        var token = "zZ1ZVyOZ2HTHC-4GUOodoXazpv8";
        var startTime = DateTime.UtcNow.AddMinutes(-5);
        var endTime = DateTime.UtcNow;

        var response = new JsonObject
        {
            ["Bets"] = new JsonArray
            {
                new JsonObject {
                    ["BetSettledDate"] = "2022-10-24T07:03:55.325",
                    ["BetStatus"] = "Opened",
                    ["BetTypeId"] = 1,
                    ["BetTypeName"] = "Single bets",
                    ["Brand"] = "GPIRMB",
                    ["ComboBonusAmount"] = 0.0,
                    ["CreationDate"] = "2022-10-24T07:03:45.407",
                    ["Currency"] = "CNY",
                    ["CustomerID"] = 108836232,
                    ["DomainID"] = 0,
                    ["FreeBet"] = new JsonObject
                    {
                        ["Amount"] = 0.0,
                        ["IsRiskFreeBet"] = 0
                    },
                    ["MerchantCustomerID"] = "4ucmqplxb029",
                    ["NonCashOutAmount"] = 0.0,
                    ["NumberOfBets"] = 1,
                    ["Odds"] = -128,
                    ["OddsDec"] = 1.78,
                    ["OddsInUserStyle"] = "1.78",
                    ["OddsStyleOfUser"] = "European",
                    ["PL"] = 0.0,
                    ["Platform"] = "Web",
                    ["PlayerLevelID"] = 10,
                    ["PlayerLevelName"] = "Test User",
                    ["PurchaseID"] = "372278979853516800",
                    ["RealMoneyAmount"] = 1.0,
                    ["Return"] = 0.0,
                    ["SearchDateTime"] = "2022-10-24T07:06:57.395",
                    ["Selections"] = new JsonArray {
                        new JsonObject  {
                            ["ActionType"]= "Real",
                            ["AwayTeam"]= "Chongqing Stars",
                            ["BetID"]= "372278979853516801",
                            ["BetType"]= "Single bets",
                            ["BonusID"]= 0,
                            ["BranchID"]= 2,
                            ["BranchName"]= "Basketball",
                            ["CouponCode"]= "",
                            ["EncodedLineID"]= "0HC372272365520138240AMM",
                            ["EventDate"]= "2022-10-24T06:25:00",
                            ["EventTypeID"]= 39,
                            ["EventTypeName"]= "Live Betting Spread",
                            ["GameID"]= 372272364224012288,
                            ["HomeTeam"]= "Henan Tigers",
                            ["IsLive"]= 1,
                            ["IsNewLine"]= 0,
                            ["LeagueID"]= 344771315745738752,
                            ["LeagueName"]= " China - New Star League",
                            ["LineID"]= 0,
                            ["LineTypeID"]= 2,
                            ["LineTypeName"]= "Spread",
                            ["LiveScore1"]= 46,
                            ["LiveScore2"]= 33,
                            ["Odds"]= -128,
                            ["OddsDec"]= 1.78,
                            ["OddsInUserStyle"]= "1.78",
                            ["Points"]= 9.5,
                            ["ReferenceID"]= 0,
                            ["RelatedBetID"]= 0,
                            ["Score"]= "None",
                            ["Status"]= "Opened",
                            ["UpdateDate"]= null,
                            ["YourBet"]= "Chongqing Stars +9.5",
                            ["isfreebet"]= 0,
                            ["isresettled"]= 0
                        }
                    },
                    ["Status"] = "Opened",
                    ["SystemName"] = "0/1",
                    ["TotalStake"] = 1.0,
                    ["UpdateDate"] = "2022-10-24T07:03:55.325",
                    ["UserName"] = "4ucmqplxb029",
                    ["ValidStake"] = 0
                }
            },
            ["LastUpdateDate"] = "2022-10-24T07:37:14.293000Z",
            ["errorCode"] = 0
        };

        var url = new Uri(apiUrl, "/get-open-bets-paging");
        var uriBuilder = new UriBuilder(url);
        var query = HttpUtility.ParseQueryString(uriBuilder.Query);
        query["token"] = token;
        uriBuilder.Query = query.ToString();
        var handler = new MockHttpMessageHandler();
        _ = handler
            .Expect(HttpMethod.Post, uriBuilder.Uri.ToString())
            .Respond(HttpStatusCode.OK, new StringContent(response.ToJsonString()));

        var fakeHttpClient = handler.ToHttpClient();
        var fakeLog = NullLogger<GameDataApiClient>.Instance;

        var sut = new GameDataApiClient(fakeHttpClient, fakeLog);

        var actual = await sut.GetOpenBetsPagingAsync(
            apiUrl: apiUrl,
            token: token,
            startTime: startTime,
            endTime: endTime)
            .ToArrayAsync()
            ?? Enumerable.Empty<BetUnsettled>();

        Assert.NotNull(actual);
        Assert.True(actual.Any());
        Assert.Equal("4ucmqplxb029", actual.First().MerchantCustomerID);
        Assert.Equal("372278979853516800", actual.First().PurchaseID);
        Assert.Equal(DateTimeOffset.Parse("2022-10-24T07:03:45.407"), actual.First().CreationDate);
        Assert.Equal(1.0m, actual.First().TotalStake);
        Assert.Equal(0.0m, actual.First().ValidStake);
        Assert.Equal(response["Bets"]?.AsArray()?.First()?.ToJsonString(), actual.First().AdditionalData);
    }
}
