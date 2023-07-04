using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Tgs.GameInterface.BTiService.Core.ApplicationServices;
using Tgs.GameInterface.BTiService.Protos;
using Tgs.ServiceUtility;
using Enum = System.Enum;

namespace Tgs.GameInterface.BTiService.GrpcServices;

internal class GameInterfaceBTiGrpcService : GameInterfaceBTiService.GameInterfaceBTiServiceBase
{
    private readonly ILogger<GameInterfaceBTiGrpcService> _logger;
    private readonly IGetService<FundInQuery, FundResult> _fundInService;
    private readonly IGetService<FundOutQuery, FundResult> _fundOutService;
    private readonly IGetService<FundStatusQuery, bool> _fundStatusService;
    private readonly ICommandService<PlayerCreateCommand> _playerCreateService;
    private readonly IGetService<PlayerBalanceQuery, PlayerBalanceResult> _playerBalanceService;
    private readonly IGetService<GameAuthTokenQuery, GameAuthTokenResult> _gameGetAuthTokenService;
    private readonly IQueryService<GameHistorySettledQuery, GameHistorySettledResult> _gameHistorySettledService;
    private readonly IQueryService<GameHistoryUnsettledQuery, GameHistoryUnsettledResult> _gameHistoryUnsettledService;

    public GameInterfaceBTiGrpcService(
        ILogger<GameInterfaceBTiGrpcService> logger,
        IGetService<FundInQuery, FundResult> fundInService,
        IGetService<FundOutQuery, FundResult> fundOutService,
        IGetService<FundStatusQuery, bool> fundStatusService,
        ICommandService<PlayerCreateCommand> playerCreateService,
        IGetService<PlayerBalanceQuery, PlayerBalanceResult> playerBalanceService,
        IGetService<GameAuthTokenQuery, GameAuthTokenResult> gameGetAuthTokenService,
        IQueryService<GameHistorySettledQuery, GameHistorySettledResult> gameHistorySettledService,
        IQueryService<GameHistoryUnsettledQuery, GameHistoryUnsettledResult> gameHistoryUnsettledService
        )
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _fundInService = fundInService ?? throw new ArgumentNullException(nameof(fundInService));
        _fundOutService = fundOutService ?? throw new ArgumentNullException(nameof(fundOutService));
        _fundStatusService = fundStatusService ?? throw new ArgumentNullException(nameof(fundStatusService));
        _playerCreateService = playerCreateService ?? throw new ArgumentNullException(nameof(playerCreateService));
        _playerBalanceService = playerBalanceService ?? throw new ArgumentNullException(nameof(playerBalanceService));
        _gameGetAuthTokenService = gameGetAuthTokenService ?? throw new ArgumentNullException(nameof(gameGetAuthTokenService));
        _gameHistorySettledService = gameHistorySettledService ?? throw new ArgumentNullException(nameof(gameHistorySettledService));
        _gameHistoryUnsettledService = gameHistoryUnsettledService ?? throw new ArgumentNullException(nameof(gameHistoryUnsettledService));
    }

    public override Task<CheckingServiceReply> CheckingService(CheckingServiceRequest request, ServerCallContext context)
    {
        _logger.LogWarning("LogOn:{logOn} Message: Create a check connection.({machineName})", DateTime.Now.ToString("o"), request.MachineName);

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

        var result = await _fundInService
                .GetAsync(
                    query: query,
                    cancellationToken: context.CancellationToken)
                .ConfigureAwait(false);

        return new FundReply()
        {
            RefTransactionCode = result.RefTransactionCode,
            TransactionId = result.TransactionId,
            Balance = result.Balance
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

        var result = await _fundOutService
                .GetAsync(
                    query: query,
                    cancellationToken: context.CancellationToken)
                .ConfigureAwait(false);

        return new FundReply()
        {
            RefTransactionCode = result.RefTransactionCode,
            TransactionId = result.TransactionId,
            Balance = result.Balance
        };
    }

    public override async Task<FundStatusReply> FundStatus(FundStatusRequest request, ServerCallContext context)
    {
        var query = new FundStatusQuery(
            ApiUrl: new(request.ApiUrl),
            AgentUserName: request.AgentUserName,
            AgentPassword: request.AgentPassword,
            RefTransactionCode: request.RefTransactionCode);

        var result = await _fundStatusService
                .GetAsync(
                    query: query,
                    cancellationToken: context.CancellationToken)
                .ConfigureAwait(false);

        return new FundStatusReply()
        {
            IsOrderExist = result
        };
    }

    public override async Task<GameAuthTokenReply> GameAuthToken(GameAuthTokenRequest request, ServerCallContext context)
    {
        var query = new GameAuthTokenQuery(
            ApiUrl: new(request.ApiUrl),
            AgentUserName: request.AgentUserName,
            AgentPassword: request.AgentPassword,
            MerchantCustomerCode: request.MerchantCustomerCode);

        var result = await _gameGetAuthTokenService
                .GetAsync(
                    query: query,
                    cancellationToken: context.CancellationToken)
                .ConfigureAwait(false);

        return new GameAuthTokenReply()
        {
            IsPlayerExist = result.IsPlayerExist,
            AuthToken = result.AuthToken
        };
    }

    public override async Task<GameHistorySettledReply> GameHistorySettled(GameHistorySettledRequest request, ServerCallContext context)
    {
        var query = new GameHistorySettledQuery(
            TokenApiUrl: new(request.TokenApiUrl),
            HistoryBetsPagingApiUrl: new(request.HistoryBetsPagingApiUrl),
            AgentUserName: request.AgentUserName,
            AgentPassword: request.AgentPassword,
            StartTime: request.StartTime.ToDateTime(),
            EndTime: request.EndTime.ToDateTime(),
            Page: request.Page,
            PageLimit: request.PageLimit);

        var gameHistorySettleds = await _gameHistorySettledService
                .QueryAsync(
                    query: query,
                    cancellationToken: context.CancellationToken)
                .ToArrayAsync(context.CancellationToken)
                .ConfigureAwait(false);
        var reply = new GameHistorySettledReply();

        if (gameHistorySettleds.Any())
        {
            foreach (var gameHistorySettled in gameHistorySettleds)
            {
                reply.Items.Add(new GameHistorySettled()
                {
                    PlayerCode = gameHistorySettled.PlayerCode,
                    ExternalRoundId = gameHistorySettled.ExternalRoundId,
                    IsClosed = gameHistorySettled.IsClosed,
                    Status = (GameHistoryStatus)Enum.Parse(typeof(GameHistoryStatus), gameHistorySettled.Status.Replace(" ", string.Empty), true),
                    ExternalCreatedOn = gameHistorySettled.ExternalCreatedOn.ToTimestamp(),
                    ExternalClosedOn = gameHistorySettled.ExternalClosedOn.ToTimestamp(),
                    Bet = gameHistorySettled.Bet,
                    ValidBet = gameHistorySettled.ValidBet,
                    PlayerWinLoss = gameHistorySettled.PlayerWinLoss,
                    Turnover = gameHistorySettled.Turnover,
                    Settle = gameHistorySettled.Settle,
                    AdditionalData = gameHistorySettled.AdditionalData
                });
            }
        }

        return reply;
    }

    public override async Task<GameHistoryUnsettledReply> GameHistoryUnsettled(GameHistoryUnsettledRequest request, ServerCallContext context)
    {
        var query = new GameHistoryUnsettledQuery(
            TokenApiUrl: new(request.TokenApiUrl),
            OpenBetsApiUrl: new(request.OpenBetsApiUrl),
            AgentUserName: request.AgentUserName,
            AgentPassword: request.AgentPassword,
            StartTime: request.StartTime.ToDateTime(),
            EndTime: request.EndTime.ToDateTime());

        var gameHistoryUnsettleds = await _gameHistoryUnsettledService
                .QueryAsync(
                    query: query,
                    cancellationToken: context.CancellationToken)
                .ToArrayAsync(context.CancellationToken)
                .ConfigureAwait(false);
        var reply = new GameHistoryUnsettledReply();

        if (gameHistoryUnsettleds.Any())
        {
            foreach (var gameHistoryUnsettled in gameHistoryUnsettleds)
            {
                reply.Items.Add(new GameHistoryUnsettled()
                {
                    PlayerCode = gameHistoryUnsettled.PlayerCode,
                    ExternalRoundId = gameHistoryUnsettled.ExternalRoundId,
                    ExternalCreatedOn = gameHistoryUnsettled.ExternalCreatedOn.ToTimestamp(),
                    Bet = gameHistoryUnsettled.Bet,
                    ValidBet = gameHistoryUnsettled.ValidBet,
                    AdditionalData = gameHistoryUnsettled.AdditionalData
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

        var result = await _playerBalanceService
                .GetAsync(
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

        await _playerCreateService
                .ExecuteAsync(
                    command: command,
                    cancellationToken: context.CancellationToken)
                .ConfigureAwait(false);

        return new BaseReply()
        {
            Status = true
        };
    }
}
