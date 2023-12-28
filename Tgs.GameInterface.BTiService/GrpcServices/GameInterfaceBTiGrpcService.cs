using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Tgs.GameInterface.BTiService.Core.ApplicationServices;
using Tgs.GameInterface.BTiService.Protos;
using Tgs.ServiceUtility;
using Enum = System.Enum;

namespace Tgs.GameInterface.BTiService.GrpcServices;

internal class GameInterfaceBTiGrpcService(
    ILogger<GameInterfaceBTiGrpcService> logger,
    IGetService<FundInQuery, FundResult> fundInService,
    IGetService<FundOutQuery, FundResult> fundOutService,
    IGetService<FundStatusQuery, bool> fundStatusService,
    ICommandService<PlayerCreateCommand> playerCreateService,
    IGetService<PlayerBalanceQuery, PlayerBalanceResult> playerBalanceService,
    IGetService<GameAuthTokenQuery, GameAuthTokenResult> gameGetAuthTokenService,
    IQueryService<GameWagerSettledQuery, GameWagerResult> gameWagerSettledService,
    IQueryService<GameWagerUnsettledQuery, GameWagerResult> gameWagerUnsettledService) : GameInterfaceBTiService.GameInterfaceBTiServiceBase
{
    public override Task<CheckingServiceReply> CheckingService(CheckingServiceRequest request, ServerCallContext context)
    {
        logger.LogWarning("LogOn:{logOn} Message: Create a check connection.({machineName})", DateTime.Now.ToString("o"), request.MachineName);

        return Task.FromResult(new CheckingServiceReply
        {
            MachineName = Environment.MachineName
        });
    }

    public override async Task<FundReply> FundIn(FundRequest request, ServerCallContext context)
    {
        var query = new FundInQuery(
            ApiUrl: new(request.ApiUrl),
            AgentUserName: request.AgentUserName,
            AgentPassword: request.AgentPassword,
            MerchantCustomerCode: request.MerchantCustomerCode,
            RefTransactionCode: request.RefTransactionCode,
            Amount: request.Amount.ToDecimal().GetValueOrDefault(0));

        var result = await fundInService.GetAsync(
            query: query,
            cancellationToken: context.CancellationToken)
            .ConfigureAwait(false);

        return new FundReply()
        {
            RefTransactionCode = result.RefTransactionCode,
            TransactionId = result.TransactionId,
            Balance = result.Balance,
            Resp = result.Resp
        };
    }

    public override async Task<FundReply> FundOut(FundRequest request, ServerCallContext context)
    {
        var query = new FundOutQuery(
            ApiUrl: new(request.ApiUrl),
            AgentUserName: request.AgentUserName,
            AgentPassword: request.AgentPassword,
            MerchantCustomerCode: request.MerchantCustomerCode,
            RefTransactionCode: request.RefTransactionCode,
            Amount: request.Amount.ToDecimal().GetValueOrDefault(0));

        var result = await fundOutService.GetAsync(
            query: query,
            cancellationToken: context.CancellationToken)
            .ConfigureAwait(false);

        return new FundReply()
        {
            RefTransactionCode = result.RefTransactionCode,
            TransactionId = result.TransactionId,
            Balance = result.Balance,
            Resp = result.Resp
        };
    }

    public override async Task<BaseReply> FundStatus(FundStatusRequest request, ServerCallContext context)
    {
        var query = new FundStatusQuery(
            ApiUrl: new(request.ApiUrl),
            AgentUserName: request.AgentUserName,
            AgentPassword: request.AgentPassword,
            RefTransactionCode: request.RefTransactionCode);

        var result = await fundStatusService.GetAsync(
            query: query,
            cancellationToken: context.CancellationToken)
            .ConfigureAwait(false);

        return new BaseReply()
        {
            Status = result
        };
    }

    public override async Task<GameAuthTokenReply> GameAuthToken(GameAuthTokenRequest request, ServerCallContext context)
    {
        var query = new GameAuthTokenQuery(
            ApiUrl: new(request.ApiUrl),
            AgentUserName: request.AgentUserName,
            AgentPassword: request.AgentPassword,
            MerchantCustomerCode: request.MerchantCustomerCode);

        var result = await gameGetAuthTokenService.GetAsync(
            query: query,
            cancellationToken: context.CancellationToken)
            .ConfigureAwait(false);

        return new GameAuthTokenReply()
        {
            IsPlayerExist = result.IsPlayerExist,
            AuthToken = result.AuthToken
        };
    }

    public override async Task<GameWagerReply> GameWagerSettled(GameWagerSettledRequest request, ServerCallContext context)
    {
        var query = new GameWagerSettledQuery(
            DataApiUrl: new(request.DataApiUrl),
            AgentUserName: request.AgentUserName,
            AgentPassword: request.AgentPassword,
            StartTime: request.StartTime.ToDateTimeOffset(),
            EndTime: request.EndTime.ToDateTimeOffset(),
            Page: request.Page,
            PageLimit: request.PageLimit);

        var gameWagers = await gameWagerSettledService.QueryAsync(
            query: query,
            cancellationToken: context.CancellationToken)
            .ToArrayAsync(context.CancellationToken)
            .ConfigureAwait(false)
            ?? Enumerable.Empty<GameWagerResult>();

        var reply = new GameWagerReply();

        if (gameWagers.Any())
        {
            foreach (var gameWager in gameWagers)
            {
                reply.Items.Add(new GameWagerInfo()
                {
                    PlayerCode = gameWager.PlayerCode,
                    ExternalWagerId = gameWager.ExternalWagerId,
                    IsClosed = gameWager.IsClosed,
                    Status = (GameHistoryStatus)Enum.Parse(typeof(GameHistoryStatus), gameWager.Status.Replace(" ", string.Empty), true),
                    ExternalCreatedOn = gameWager.ExternalCreatedOn.ToTimestamp(),
                    ExternalClosedOn = gameWager.ExternalClosedOn.ToTimestamp(),
                    Bet = gameWager.Bet,
                    ValidBet = gameWager.ValidBet,
                    Settle = gameWager.Settle,
                    WinLoss = gameWager.WinLoss,
                    AdditionalData = gameWager.AdditionalData
                });
            }
        }

        return reply;
    }

    public override async Task<GameWagerReply> GameWagerUnsettled(GameWagerUnsettledRequest request, ServerCallContext context)
    {
        var query = new GameWagerUnsettledQuery(
            DataApiUrl: new(request.DataApiUrl),
            AgentUserName: request.AgentUserName,
            AgentPassword: request.AgentPassword,
            StartTime: request.StartTime.ToDateTimeOffset(),
            EndTime: request.EndTime.ToDateTimeOffset());

        var gameWagers = await gameWagerUnsettledService.QueryAsync(
            query: query,
            cancellationToken: context.CancellationToken)
            .ToArrayAsync(context.CancellationToken)
            .ConfigureAwait(false)
            ?? Enumerable.Empty<GameWagerResult>();

        var reply = new GameWagerReply();

        if (gameWagers.Any())
        {
            foreach (var gameWager in gameWagers)
            {
                reply.Items.Add(new GameWagerInfo()
                {
                    PlayerCode = gameWager.PlayerCode,
                    ExternalWagerId = gameWager.ExternalWagerId,
                    IsClosed = gameWager.IsClosed,
                    Status = (GameHistoryStatus)Enum.Parse(typeof(GameHistoryStatus), gameWager.Status.Replace(" ", string.Empty), true),
                    ExternalCreatedOn = gameWager.ExternalCreatedOn.ToTimestamp(),
                    ExternalClosedOn = gameWager.ExternalClosedOn.ToTimestamp(),
                    Bet = gameWager.Bet,
                    ValidBet = gameWager.ValidBet,
                    Settle = gameWager.Settle,
                    WinLoss = gameWager.WinLoss,
                    AdditionalData = gameWager.AdditionalData
                });
            }
        }

        return reply;
    }

    public override async Task<PlayerBalanceReply> PlayerBalance(PlayerBalanceRequest request, ServerCallContext context)
    {
        var query = new PlayerBalanceQuery(
            ApiUrl: new(request.ApiUrl),
            AgentUserName: request.AgentUserName,
            AgentPassword: request.AgentPassword,
            MerchantCustomerCode: request.MerchantCustomerCode);

        var result = await playerBalanceService.GetAsync(
            query: query,
            cancellationToken: context.CancellationToken)
            .ConfigureAwait(false);

        return new PlayerBalanceReply()
        {
            IsPlayerExist = result.IsPlayerExist,
            Balance = result.Balance
        };
    }

    public override async Task<BaseReply> PlayerCreate(PlayerCreateRequest request, ServerCallContext context)
    {
        var command = new PlayerCreateCommand(
            ApiUrl: new(request.ApiUrl),
            AgentUserName: request.AgentUserName,
            AgentPassword: request.AgentPassword,
            BrandCode: request.BrandCode,
            MerchantCustomerCode: request.MerchantCustomerCode,
            Currency: request.Currency);

        await playerCreateService.ExecuteAsync(
            command: command,
            cancellationToken: context.CancellationToken)
            .ConfigureAwait(false);

        return new BaseReply()
        {
            Status = true
        };
    }
}
