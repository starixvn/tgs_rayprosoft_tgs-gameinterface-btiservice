using System.Text.Json.Nodes;
using NSubstitute;
using Tgs.GameInterface.BTiService.Core.ApplicationServices;
using Tgs.GameInterface.BTiService.Core.Models;

namespace Tgs.GameInterface.BTiService.Core.UnitTest.ApplicationServices;

public class GameWagerUnsettledQueryServiceUnitTest
{
    [Fact]
    public async Task Opened()
    {
        var query = new GameWagerUnsettledQuery(
            DataApiUrl: new Uri("https://data-api.442hattrick.com/"),
            AgentUserName: "PegaGaming_GPIRMB",
            AgentPassword: "IM60Ho7Y4B9C",
            StartTime: DateTime.UtcNow.AddMinutes(-10),
            EndTime: DateTime.UtcNow);

        var fakeClient = Substitute.For<IGameDataApiClient>();

        var token = Guid.NewGuid().ToString();
        _ = fakeClient.GetTokenAsync(
            apiUrl: Arg.Is(query.DataApiUrl),
            agentUserName: Arg.Is(query.AgentUserName),
            agentPassword: Arg.Is(query.AgentPassword),
            cancellationToken: Arg.Any<CancellationToken>())
            .Returns(token);

        var betSettleds = new BetUnsettled[]
        {
            new(
                MerchantCustomerID:"4uhfooes94j9",
                PurchaseID:"504134723166224384",
                BetStatus:"Opened",
                CreationDate:DateTimeOffset.Parse("2023-10-23T03:31:24.365"),
                TotalStake:100.00m,
                ValidStake:0.00m,
                AdditionalData: new JsonObject{
                    ["BetSettledDate"] = "2023-10-23T03:31:24.471",
                    ["BetStatus"] = "Opened",
                    ["BetTypeId"] = 1,
                    ["BetTypeName"] = "Single bets",
                    ["Brand"] = "TGSPB1",
                    ["ComboBonusAmount"] = 0.0,
                    ["CreationDate"] = "2023-10-23T03:31:24.365",
                    ["Currency"] = "PHP",
                    ["CustomerID"] = 109336096,
                    ["DomainID"] = 0,
                    ["FreeBet"] =new JsonObject {
                        ["Amount"] = 0,
                        ["IsRiskFreeBet"] = 0
                    },
                    ["Gain"] = "188",
                    ["MerchantCustomerID"] = "4uhfooes94j9",
                    ["NonCashOutAmount"] = 0.0,
                    ["Odds"] = -114,
                    ["OddsDec"] = 1.88,
                    ["OddsInUserStyle"] = "1.88",
                    ["OddsStyleOfUser"] = "European",
                    ["PL"] = 0.0,
                    ["Platform"] = "Mobile",
                    ["PlayerLevelID"] = 0,
                    ["PlayerLevelName"] = "Newly Registered",
                    ["PurchaseID"] = "504134723166224384",
                    ["RealMoneyAmount"] = 100.0,
                    ["Return"] = 0.0,
                    ["SearchDateTime"] = "2023-10-23T03:37:52.481",
                    ["Selections"] =new JsonArray{
                        new JsonObject{
                            ["ActionType"] = "Real",
                            ["AwayTeam"] = "Daqing [W]",
                            ["BetID"] = "504134723166224385",
                            ["BetType"] = "Single bets",
                            ["BonusID"] = 0,
                            ["BranchID"] = 2,
                            ["BranchName"] = "Basketball",
                            ["CouponCode"] = "",
                            ["EncodedLineID"] = "0HC503917120590221312AMM",
                            ["EventDate"] = "2023-10-23T11:30:00",
                            ["EventTypeID"] = 0,
                            ["EventTypeName"] = "FT Spread",
                            ["GameID"] = 503913968327512064,
                            ["HomeTeam"] = "Zhejiang [W]",
                            ["IsLive"] = 0,
                            ["IsNewLine"] = 0,
                            ["LeagueID"] = 503114191121731584,
                            ["LeagueName"] = "China - WCBA",
                            ["LineID"] = 0,
                            ["LineTypeID"] = 2,
                            ["LineTypeName"] = "Spread",
                            ["LiveScore1"] = 0,
                            ["LiveScore2"] = 0,
                            ["Odds"] = -114,
                            ["OddsDec"] = 1.88,
                            ["OddsInUserStyle"] = "1.88",
                            ["Points"] = 30.5,
                            ["ReferenceID"] = 0,
                            ["RelatedBetID"] = 0,
                            ["Score"] = "None",
                            ["Status"] = "Opened",
                            ["UpdateDate"] = null,
                            ["YourBet"] = "Daqing [W] +30.5",
                            ["Isfreebet"] = 0
                        }
                    },
                    ["Status"] = "0/1",
                    ["TotalStake"] = 100.0,
                    ["UpdateDate"] = "2023-10-23T03:31:24.471",
                    ["UserName"] = "4uhfooes94j9",
                    ["ValidStake"] = 0
                }.ToJsonString()  )
        };
        _ = fakeClient
            .GetOpenBetsPagingAsync(
            apiUrl: Arg.Is(query.DataApiUrl),
            token: Arg.Is(token),
            startTime: Arg.Is(query.StartTime),
            endTime: Arg.Is(query.EndTime),
            cancellationToken: Arg.Any<CancellationToken>())
            .Returns(betSettleds.ToAsyncEnumerable());

        var service = new GameWagerUnsettledQueryService(
            fakeClient);

        var actual = await service.QueryAsync(
            query: query)
            .ToArrayAsync()
            ?? Enumerable.Empty<GameWagerResult>();

        Assert.NotNull(actual);
        Assert.True(actual.Any());
        Assert.Equal(betSettleds.First().MerchantCustomerID, actual.First().PlayerCode);
        Assert.Equal(betSettleds.First().PurchaseID, actual.First().ExternalWagerId);
        Assert.False(actual.First().IsClosed);
        Assert.Equal("Opened", actual.First().Status);
        Assert.Equal(betSettleds.First().CreationDate, actual.First().ExternalCreatedOn);
        Assert.Equal(betSettleds.First().TotalStake, actual.First().Bet);
        Assert.Equal(betSettleds.First().ValidStake, actual.First().ValidBet);
        Assert.Equal(betSettleds.First().AdditionalData, actual.First().AdditionalData);
    }
}
