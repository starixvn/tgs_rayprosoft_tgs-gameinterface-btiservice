using System.Text.Json.Nodes;
using NSubstitute;
using Tgs.GameInterface.BTiService.Core.ApplicationServices;
using Tgs.GameInterface.BTiService.Core.Models;

namespace Tgs.GameInterface.BTiService.Core.UnitTest.ApplicationServices;

public class GameWagerSettledQueryServiceUnitTest
{
    [Fact]
    public async Task Won()
    {
        var query = new GameWagerSettledQuery(
            DataApiUrl: new Uri("https://data-api.442hattrick.com/"),
            AgentUserName: "PegaGaming_GPIRMB",
            AgentPassword: "IM60Ho7Y4B9C",
            StartTime: DateTime.UtcNow.AddMinutes(-10),
            EndTime: DateTime.UtcNow,
            Page: 0,
            PageLimit: 1000);

        var fakeClient = Substitute.For<IGameDataApiClient>();

        var token = Guid.NewGuid().ToString();
        _ = fakeClient.GetTokenAsync(
            apiUrl: Arg.Is(query.DataApiUrl),
            agentUserName: Arg.Is(query.AgentUserName),
            agentPassword: Arg.Is(query.AgentPassword),
            cancellationToken: Arg.Any<CancellationToken>())
            .Returns(token);

        var betSettleds = new BetSettled[]
        {
            new BetSettled(
                MerchantCustomerID:"4uhtdyoha5d2",
                PurchaseID:"504126437851365376",
                BetStatus:"Won",
                CreationDate:DateTimeOffset.Parse("2023-10-23T02:58:28.987"),
                BetSettledDate:DateTimeOffset.Parse("2023-10-23T03:37:09.503"),
                TotalStake:530.00m,
                ValidStake:530.00m,
                PL:424.00m,
                Return:954.00m,
                AdditionalData: new JsonObject{
                    ["BetSettledDate"] = "2023-10-23T03:37:09.503",
                    ["BetStatus"] = "Won",
                    ["BetTypeId"] = 1,
                    ["BetTypeName"] = "Single bets",
                    ["Brand"] = "TGSPB1",
                    ["ComboBonusAmount"] = 0,
                    ["CreationDate"] = "2023-10-23T02:58:28.987",
                    ["Currency"] = "PHP",
                    ["CustomerID"] = 109331861,
                    ["DomainID"] = 0,
                    ["FreeBet"] =new JsonObject {
                        ["Amount"] = 0,
                        ["IsRiskFreeBet"] = 0
                    },
                    ["MerchantCustomerID"] = "4uhtdyoha5d2",
                    ["NonCashOutAmount"] = 0,
                    ["Odds"] = -125,
                    ["OddsDec"] = 1.8,
                    ["OddsInUserStyle"] = "1.8",
                    ["OddsStyleOfUser"] = "European",
                    ["PL"] = 424,
                    ["Platform"] = "Mobile",
                    ["PlayerLevelID"] = 0,
                    ["PlayerLevelName"] = "Newly Registered",
                    ["PurchaseID"] = "504126437851365376",
                    ["RealMoneyAmount"] = 530,
                    ["Return"] = 954,
                    ["SearchDateTime"] = "2023-10-23T03:43:56.867",
                    ["Selections"] =new JsonArray{
                        new JsonObject{
                            ["ActionType"] = "Real",
                            ["AwayTeam"] = "Volga",
                            ["BetID"] = "504126437851365377",
                            ["BetType"] = "Single bets",
                            ["BonusID"] = 0,
                            ["BranchID"] = 2,
                            ["BranchName"] = "Basketball",
                            ["CouponCode"] = "",
                            ["EncodedLineID"] = "0HC504121737282359296AMM",
                            ["EventDate"] = "2023-10-23T02:45:00",
                            ["EventTypeID"] = 39,
                            ["EventTypeName"] = "Live Betting Spread",
                            ["GameID"] = 504121736468680700,
                            ["HomeTeam"] = "Yenisei",
                            ["IsLive"] = 1,
                            ["IsNewLine"] = 0,
                            ["LeagueID"] = 464769465809428500,
                            ["LeagueName"] = "Russia - IPBL Pro Division",
                            ["LineID"] = 0,
                            ["LineTypeID"] = 2,
                            ["LineTypeName"] = "Spread",
                            ["LiveScore1"] = 23,
                            ["LiveScore2"] = 27,
                            ["Odds"] = -125,
                            ["OddsDec"] = 1.8,
                            ["OddsInUserStyle"] = "1.8",
                            ["Points"] = -3.5,
                            ["ReferenceID"] = 0,
                            ["RelatedBetID"] = 0,
                            ["Score"] = "83 : 91",
                            ["Status"] = "Won",
                            ["UpdateDate"] = null,
                            ["YourBet"] = "Volga -3.5",
                            ["Isfreebet"] = 0,
                            ["Isresettled"] = 0
                        }
                    },
                    ["Status"] = "Won",
                    ["SystemName"] = "0/1",
                    ["TotalStake"] = 530,
                    ["UpdateDate"] = "2023-10-23T03:37:09.503",
                    ["UserName"] = "4uhtdyoha5d2",
                    ["ValidStake"] = 530
                }.ToJsonString()  )
        };
        _ = fakeClient
            .GetHistoryBetsPagingAsync(
            apiUrl: Arg.Is(query.DataApiUrl),
            token: Arg.Is(token),
            startTime: Arg.Is(query.StartTime),
            endTime: Arg.Is(query.EndTime),
            page: Arg.Is(query.Page),
            pageLimit: Arg.Is(query.PageLimit),
            cancellationToken: Arg.Any<CancellationToken>())
            .Returns(betSettleds.ToAsyncEnumerable());

        var service = new GameWagerSettledQueryService(
            fakeClient);

        var actual = await service.QueryAsync(
            query: query)
            .ToArrayAsync()
            ?? Enumerable.Empty<GameWagerResult>();

        Assert.NotNull(actual);
        Assert.True(actual.Any());
        Assert.Equal(betSettleds.First().MerchantCustomerID, actual.First().PlayerCode);
        Assert.Equal(betSettleds.First().PurchaseID, actual.First().ExternalWagerId);
        Assert.True(actual.First().IsClosed);
        Assert.Equal("Won", actual.First().Status);
        Assert.Equal(betSettleds.First().CreationDate, actual.First().ExternalCreatedOn);
        Assert.Equal(betSettleds.First().BetSettledDate, actual.First().ExternalClosedOn);
        Assert.Equal(betSettleds.First().TotalStake, actual.First().Bet);
        Assert.Equal(betSettleds.First().ValidStake, actual.First().ValidBet);
        Assert.Equal(betSettleds.First().Return, actual.First().Settle);
        Assert.Equal(betSettleds.First().PL, actual.First().WinLoss);
        Assert.Equal(betSettleds.First().AdditionalData, actual.First().AdditionalData);
    }

    [Fact]
    public async Task Lost()
    {
        var query = new GameWagerSettledQuery(
            DataApiUrl: new Uri("https://data-api.442hattrick.com/"),
            AgentUserName: "PegaGaming_GPIRMB",
            AgentPassword: "IM60Ho7Y4B9C",
            StartTime: DateTime.UtcNow.AddMinutes(-10),
            EndTime: DateTime.UtcNow,
            Page: 0,
            PageLimit: 1000);

        var fakeClient = Substitute.For<IGameDataApiClient>();

        var token = Guid.NewGuid().ToString();
        _ = fakeClient.GetTokenAsync(
            apiUrl: Arg.Is(query.DataApiUrl),
            agentUserName: Arg.Is(query.AgentUserName),
            agentPassword: Arg.Is(query.AgentPassword),
            cancellationToken: Arg.Any<CancellationToken>())
            .Returns(token);

        var betSettleds = new BetSettled[]
        {
            new BetSettled(
                MerchantCustomerID:"4uhtdyoha5d2",
                PurchaseID:"504149378416721920",
                BetStatus:"Lost",
                CreationDate:DateTimeOffset.Parse("2023-10-23T04:29:38.446"),
                BetSettledDate:DateTimeOffset.Parse("2023-10-23T04:51:36.359"),
                TotalStake:654.00m,
                ValidStake:654.00m,
                PL:-654.00m,
                Return:0.00m,
                AdditionalData: new JsonObject{
                    ["BetSettledDate"] = "2023-10-23T04:51:36.359",
                    ["BetStatus"] = "Lost",
                    ["BetTypeId"] = 2,
                    ["BetTypeName"] = "Combo bets",
                    ["Brand"] = "TGSPB1",
                    ["ComboBonusAmount"] = 0,
                    ["CreationDate"] = "2023-10-23T04:29:38.446",
                    ["Currency"] = "PHP",
                    ["CustomerID"] = 109331861,
                    ["DomainID"] = 0,
                    ["FreeBet"] =new JsonObject {
                        ["Amount"] = 0,
                        ["IsRiskFreeBet"] = 0
                    },
                    ["MerchantCustomerID"] = "4uhtdyoha5d2",
                    ["NonCashOutAmount"] = 0,
                    ["Odds"] = 240,
                    ["OddsDec"] = 3.4,
                    ["OddsInUserStyle"] = "3.4",
                    ["OddsStyleOfUser"] = "European",
                    ["PL"] = -654,
                    ["Platform"] = "Mobile",
                    ["PlayerLevelID"] = 0,
                    ["PlayerLevelName"] = "Newly Registered",
                    ["PurchaseID"] = "504149378416721920",
                    ["RealMoneyAmount"] = 654,
                    ["Return"] = 0,
                    ["SearchDateTime"] = "2023-10-23T04:58:55.346",
                    ["Selections"] =new JsonArray{
                        new JsonObject{
                            ["ActionType"] = "Real",
                            ["AwayTeam"] = "Orenburg [W]",
                            ["BetID"] = "504149378416721921",
                            ["BetType"] = "Combo bets",
                            ["BonusID"] = 0,
                            ["BranchID"] = 2,
                            ["BranchName"] = "Basketball",
                            ["CouponCode"] = "",
                            ["EncodedLineID"] = "0HC504139068704329728AMM",
                            ["EventDate"] = "2023-10-23T03:50:00",
                            ["EventTypeID"] = 39,
                            ["EventTypeName"] = "Live Betting Spread",
                            ["GameID"] = 504139067760611300,
                            ["HomeTeam"] = "SFU Krasnoyarsk [W]",
                            ["IsLive"] = 1,
                            ["IsNewLine"] = 0,
                            ["LeagueID"] = 468690857294667800,
                            ["LeagueName"] = "Russia - IPBL Pro Division Women",
                            ["LineID"] = 0,
                            ["LineTypeID"] = 2,
                            ["LineTypeName"] = "Spread",
                            ["LiveScore1"] = 48,
                            ["LiveScore2"] = 57,
                            ["Odds"] = -128,
                            ["OddsDec"] = 1.78,
                            ["OddsInUserStyle"] = "1.78",
                            ["Points"] = -7.5,
                            ["ReferenceID"] = 0,
                            ["RelatedBetID"] = 0,
                            ["Score"] = "81 : 88",
                            ["Status"] = "Lost",
                            ["UpdateDate"] = null,
                            ["YourBet"] = "Orenburg [W] -7.5",
                            ["Isfreebet"] = 0,
                            ["Isresettled"] = 0
                        },
                        new JsonObject{
                            ["ActionType"] = "Real",
                            ["AwayTeam"] = "Changwon LG Sakers",
                            ["BetID"] = "504149378416721921",
                            ["BetType"] = "Combo bets",
                            ["BonusID"] = 0,
                            ["BranchID"] = 2,
                            ["BranchName"] = "Basketball",
                            ["CouponCode"] = "",
                            ["EncodedLineID"] = "0HC501549388511059968HMM",
                            ["EventDate"] = "2023-10-23T10:00:00",
                            ["EventTypeID"] = 0,
                            ["EventTypeName"] = "FT Spread",
                            ["GameID"] = 501549386111946750,
                            ["HomeTeam"] = "Mobis Phoebus",
                            ["IsLive"] = 0,
                            ["IsNewLine"] = 0,
                            ["LeagueID"] = 501014849271373800,
                            ["LeagueName"] = "South Korea - KBL",
                            ["LineID"] = 0,
                            ["LineTypeID"] = 2,
                            ["LineTypeName"] = "Spread",
                            ["LiveScore1"] = 0,
                            ["LiveScore2"] = 0,
                            ["Odds"] = -110,
                            ["OddsDec"] = 1.91,
                            ["OddsInUserStyle"] = "1.91",
                            ["Points"] = -4,
                            ["ReferenceID"] = 0,
                            ["RelatedBetID"] = 0,
                            ["Score"] = "None",
                            ["Status"] = "Opened",
                            ["UpdateDate"] = null,
                            ["YourBet"] = "Mobis Phoebus -4",
                            ["Isfreebet"] = 0,
                            ["Isresettled"] = 0
                        }
                    },
                    ["Status"] = "Lost",
                    ["SystemName"] = "",
                    ["TotalStake"] = 654,
                    ["UpdateDate"] = "2023-10-23T04:51:36.359",
                    ["UserName"] = "4uhtdyoha5d2",
                    ["ValidStake"] = 654
                }.ToJsonString()  )
        };
        _ = fakeClient
            .GetHistoryBetsPagingAsync(
            apiUrl: Arg.Is(query.DataApiUrl),
            token: Arg.Is(token),
            startTime: Arg.Is(query.StartTime),
            endTime: Arg.Is(query.EndTime),
            page: Arg.Is(query.Page),
            pageLimit: Arg.Is(query.PageLimit),
            cancellationToken: Arg.Any<CancellationToken>())
            .Returns(betSettleds.ToAsyncEnumerable());

        var service = new GameWagerSettledQueryService(
            fakeClient);

        var actual = await service.QueryAsync(
            query: query)
            .ToArrayAsync()
            ?? Enumerable.Empty<GameWagerResult>();

        Assert.NotNull(actual);
        Assert.True(actual.Any());
        Assert.Equal(betSettleds.First().MerchantCustomerID, actual.First().PlayerCode);
        Assert.Equal(betSettleds.First().PurchaseID, actual.First().ExternalWagerId);
        Assert.True(actual.First().IsClosed);
        Assert.Equal("Lost", actual.First().Status);
        Assert.Equal(betSettleds.First().CreationDate, actual.First().ExternalCreatedOn);
        Assert.Equal(betSettleds.First().BetSettledDate, actual.First().ExternalClosedOn);
        Assert.Equal(betSettleds.First().TotalStake, actual.First().Bet);
        Assert.Equal(betSettleds.First().ValidStake, actual.First().ValidBet);
        Assert.Equal(betSettleds.First().Return, actual.First().Settle);
        Assert.Equal(betSettleds.First().PL, actual.First().WinLoss);
        Assert.Equal(betSettleds.First().AdditionalData, actual.First().AdditionalData);
    }

    [Fact]
    public async Task Cashout()
    {
        var query = new GameWagerSettledQuery(
            DataApiUrl: new Uri("https://data-api.442hattrick.com/"),
            AgentUserName: "PegaGaming_GPIRMB",
            AgentPassword: "IM60Ho7Y4B9C",
            StartTime: DateTime.UtcNow.AddMinutes(-10),
            EndTime: DateTime.UtcNow,
            Page: 0,
            PageLimit: 1000);

        var fakeClient = Substitute.For<IGameDataApiClient>();

        var token = Guid.NewGuid().ToString();
        _ = fakeClient.GetTokenAsync(
            apiUrl: Arg.Is(query.DataApiUrl),
            agentUserName: Arg.Is(query.AgentUserName),
            agentPassword: Arg.Is(query.AgentPassword),
            cancellationToken: Arg.Any<CancellationToken>())
            .Returns(token);

        var betSettleds = new BetSettled[]
        {
            new BetSettled(
                MerchantCustomerID:"4uh8eeckt0ie",
                PurchaseID:"503894265982767104",
                BetStatus:"Cashout",
                CreationDate:DateTimeOffset.Parse("2023-10-22T11:35:55.203"),
                BetSettledDate:DateTimeOffset.Parse("2023-10-22T11:42:58.362"),
                TotalStake:24.00m,
                ValidStake:0.00m,
                PL:-13.79m,
                Return:10.21m,
                AdditionalData: new JsonObject{
                    ["BetSettledDate"] = "2023-10-22T11:42:58.362",
                    ["BetStatus"] = "Cashout",
                    ["BetTypeId"] = 1,
                    ["BetTypeName"] = "Single bets",
                    ["Brand"] = "TGSPB1",
                    ["ComboBonusAmount"] = 0,
                    ["CreationDate"] = "2023-10-22T11:35:55.203",
                    ["Currency"] = "PHP",
                    ["CustomerID"] = 109325161,
                    ["DomainID"] = 0,
                    ["FreeBet"] =new JsonObject {
                        ["Amount"] = 0,
                        ["IsRiskFreeBet"] = 0
                    },
                    ["MerchantCustomerID"] = "4uh8eeckt0ie",
                    ["NonCashOutAmount"] = 0,
                    ["Odds"] = 154,
                    ["OddsDec"] = 2.54,
                    ["OddsInUserStyle"] = "2.54",
                    ["OddsStyleOfUser"] = "European",
                    ["PL"] = -13.79,
                    ["Platform"] = "Mobile",
                    ["PlayerLevelID"] = 0,
                    ["PlayerLevelName"] = "Newly Registered",
                    ["PurchaseID"] = "503894265982767104",
                    ["RealMoneyAmount"] = 24,
                    ["Return"] = 10.21,
                    ["SearchDateTime"] = "2023-10-22T11:50:33.538",
                    ["Selections"] =new JsonArray{
                        new JsonObject{
                            ["ActionType"] = "Real",
                            ["AwayTeam"] = "Shanghai Sharks",
                            ["BetID"] = "503894265982767105",
                            ["BetType"] = "Single bets",
                            ["BonusID"] = 0,
                            ["BranchID"] = 2,
                            ["BranchName"] = "Basketball",
                            ["CouponCode"] = "",
                            ["EncodedLineID"] = "0ML501274952251744256A",
                            ["EventDate"] = "2023-10-22T11:35:00",
                            ["EventTypeID"] = 654,
                            ["EventTypeName"] = "1st Quarter Moneyline",
                            ["GameID"] = 501274949559001100,
                            ["HomeTeam"] = "Zhejiang Chouzhou",
                            ["IsLive"] = 1,
                            ["IsNewLine"] = 0,
                            ["LeagueID"] = 500935316652478460,
                            ["LeagueName"] = "China - CBA",
                            ["LineID"] = 0,
                            ["LineTypeID"] = 1,
                            ["LineTypeName"] = "Moneyline",
                            ["LiveScore1"] = 0,
                            ["LiveScore2"] = 0,
                            ["Odds"] = 154,
                            ["OddsDec"] = 2.54,
                            ["OddsInUserStyle"] = "2.54",
                            ["Points"] = 0,
                            ["ReferenceID"] = 0,
                            ["RelatedBetID"] = 0,
                            ["Score"] = "None",
                            ["Status"] = "Cashout",
                            ["UpdateDate"] = null,
                            ["YourBet"] = "Shanghai Sharks",
                            ["Isfreebet"] = 0,
                            ["Isresettled"] = 0
                        }
                    },
                    ["Status"] = "Cashout",
                    ["SystemName"] = "0/1",
                    ["TotalStake"] = 24,
                    ["UpdateDate"] = "2023-10-22T11:42:58.362",
                    ["UserName"] = "4uh8eeckt0ie",
                    ["ValidStake"] = 0
                }.ToJsonString()  )
        };
        _ = fakeClient
            .GetHistoryBetsPagingAsync(
            apiUrl: Arg.Is(query.DataApiUrl),
            token: Arg.Is(token),
            startTime: Arg.Is(query.StartTime),
            endTime: Arg.Is(query.EndTime),
            page: Arg.Is(query.Page),
            pageLimit: Arg.Is(query.PageLimit),
            cancellationToken: Arg.Any<CancellationToken>())
            .Returns(betSettleds.ToAsyncEnumerable());

        var service = new GameWagerSettledQueryService(
            fakeClient);

        var actual = await service.QueryAsync(
            query: query)
            .ToArrayAsync()
            ?? Enumerable.Empty<GameWagerResult>();

        Assert.NotNull(actual);
        Assert.True(actual.Any());
        Assert.Equal(betSettleds.First().MerchantCustomerID, actual.First().PlayerCode);
        Assert.Equal(betSettleds.First().PurchaseID, actual.First().ExternalWagerId);
        Assert.True(actual.First().IsClosed);
        Assert.Equal("Cashout", actual.First().Status);
        Assert.Equal(betSettleds.First().CreationDate, actual.First().ExternalCreatedOn);
        Assert.Equal(betSettleds.First().BetSettledDate, actual.First().ExternalClosedOn);
        Assert.Equal(betSettleds.First().TotalStake, actual.First().Bet);
        Assert.Equal(betSettleds.First().ValidStake, actual.First().ValidBet);
        Assert.Equal(betSettleds.First().Return, actual.First().Settle);
        Assert.Equal(betSettleds.First().PL, actual.First().WinLoss);
        Assert.Equal(betSettleds.First().AdditionalData, actual.First().AdditionalData);
    }

    [Fact]
    public async Task Canceled()
    {
        var query = new GameWagerSettledQuery(
            DataApiUrl: new Uri("https://data-api.442hattrick.com/"),
            AgentUserName: "PegaGaming_GPIRMB",
            AgentPassword: "IM60Ho7Y4B9C",
            StartTime: DateTime.UtcNow.AddMinutes(-10),
            EndTime: DateTime.UtcNow,
            Page: 0,
            PageLimit: 1000);

        var fakeClient = Substitute.For<IGameDataApiClient>();

        var token = Guid.NewGuid().ToString();
        _ = fakeClient.GetTokenAsync(
            apiUrl: Arg.Is(query.DataApiUrl),
            agentUserName: Arg.Is(query.AgentUserName),
            agentPassword: Arg.Is(query.AgentPassword),
            cancellationToken: Arg.Any<CancellationToken>())
            .Returns(token);

        var betSettleds = new BetSettled[]
        {
            new BetSettled(
                MerchantCustomerID:"4uhfooes94j9",
                PurchaseID:"503704871078350848",
                BetStatus:"Canceled",
                CreationDate:DateTimeOffset.Parse("2023-10-21T23:03:19.917"),
                BetSettledDate:DateTimeOffset.Parse("2023-10-22T12:03:24.475"),
                TotalStake:100.00m,
                ValidStake:0.00m,
                PL: 0.00m,
                Return:100.00m,
                AdditionalData: new JsonObject{
                    ["BetSettledDate"] = "2023-10-22T12:03:24.475",
                    ["BetStatus"] = "Canceled",
                    ["BetTypeId"] = 1,
                    ["BetTypeName"] = "Single bets",
                    ["Brand"] = "TGSPB1",
                    ["ComboBonusAmount"] = 0,
                    ["CreationDate"] = "2023-10-21T23:03:19.917",
                    ["Currency"] = "PHP",
                    ["CustomerID"] = 109336096,
                    ["DomainID"] = 0,
                    ["FreeBet"] =new JsonObject {
                        ["Amount"] = 0,
                        ["IsRiskFreeBet"] = 0
                    },
                    ["MerchantCustomerID"] = "4uhfooes94j9",
                    ["NonCashOutAmount"] = 0,
                    ["Odds"] = -120,
                    ["OddsDec"] = 1.83,
                    ["OddsInUserStyle"] = "1.83",
                    ["OddsStyleOfUser"] = "European",
                    ["PL"] = 0,
                    ["Platform"] = "Mobile",
                    ["PlayerLevelID"] = 0,
                    ["PlayerLevelName"] = "Newly Registered",
                    ["PurchaseID"] = "503704871078350848",
                    ["RealMoneyAmount"] = 100,
                    ["Return"] = 100,
                    ["SearchDateTime"] = "2023-10-22T12:11:01.126",
                    ["Selections"] =new JsonArray{
                        new JsonObject{
                            ["ActionType"] = "Real",
                            ["AwayTeam"] = "ITD Santa Tecla F1 [W]",
                            ["BetID"] = "503704871078350849",
                            ["BetType"] = "Single bets",
                            ["BonusID"] = 0,
                            ["BranchID"] = 2,
                            ["BranchName"] = "Basketball",
                            ["CouponCode"] = "",
                            ["EncodedLineID"] = "0OU503500126464344112OMM",
                            ["EventDate"] = "2023-10-22T00:00:00",
                            ["EventTypeID"] = 623,
                            ["EventTypeName"] = "1st Half O/U",
                            ["GameID"] = 503500124761395200,
                            ["HomeTeam"] = "Teclenas [W]",
                            ["IsLive"] = 0,
                            ["IsNewLine"] = 0,
                            ["LeagueID"] = 479971947569250300,
                            ["LeagueName"] = "El Salvador - Liga Women",
                            ["LineID"] = 0,
                            ["LineTypeID"] = 3,
                            ["LineTypeName"] = "O/U",
                            ["LiveScore1"] = 0,
                            ["LiveScore2"] = 0,
                            ["Odds"] = -120,
                            ["OddsDec"] = 1.83,
                            ["OddsInUserStyle"] = "1.83",
                            ["Points"] = 55.5,
                            ["ReferenceID"] = 0,
                            ["RelatedBetID"] = 0,
                            ["Score"] = "None",
                            ["Status"] = "Canceled",
                            ["UpdateDate"] = null,
                            ["YourBet"] = "Over 55.5",
                            ["Isfreebet"] = 0,
                            ["Isresettled"] = 0
                        }
                    },
                    ["Status"] = "Canceled",
                    ["SystemName"] = "0/1",
                    ["TotalStake"] = 100,
                    ["UpdateDate"] = "2023-10-22T12:03:24.475",
                    ["UserName"] = "4uhfooes94j9",
                    ["ValidStake"] = 0
                }.ToJsonString()  )
        };
        _ = fakeClient
            .GetHistoryBetsPagingAsync(
            apiUrl: Arg.Is(query.DataApiUrl),
            token: Arg.Is(token),
            startTime: Arg.Is(query.StartTime),
            endTime: Arg.Is(query.EndTime),
            page: Arg.Is(query.Page),
            pageLimit: Arg.Is(query.PageLimit),
            cancellationToken: Arg.Any<CancellationToken>())
            .Returns(betSettleds.ToAsyncEnumerable());

        var service = new GameWagerSettledQueryService(
            fakeClient);

        var actual = await service.QueryAsync(
            query: query)
            .ToArrayAsync()
            ?? Enumerable.Empty<GameWagerResult>();

        Assert.NotNull(actual);
        Assert.True(actual.Any());
        Assert.Equal(betSettleds.First().MerchantCustomerID, actual.First().PlayerCode);
        Assert.Equal(betSettleds.First().PurchaseID, actual.First().ExternalWagerId);
        Assert.True(actual.First().IsClosed);
        Assert.Equal("Canceled", actual.First().Status);
        Assert.Equal(betSettleds.First().CreationDate, actual.First().ExternalCreatedOn);
        Assert.Equal(betSettleds.First().BetSettledDate, actual.First().ExternalClosedOn);
        Assert.Equal(betSettleds.First().TotalStake, actual.First().Bet);
        Assert.Equal(betSettleds.First().ValidStake, actual.First().ValidBet);
        Assert.Equal(betSettleds.First().Return, actual.First().Settle);
        Assert.Equal(betSettleds.First().PL, actual.First().WinLoss);
        Assert.Equal(betSettleds.First().AdditionalData, actual.First().AdditionalData);
    }
}
