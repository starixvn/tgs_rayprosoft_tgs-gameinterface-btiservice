using Grpc.Core;
using Tgs.GameInterface.BTiService.Core.ApplicationServices;
using Tgs.GameInterface.BTiService.Protos;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.BTiService.GrpcServices;

public class GameInterfaceBTiGrpcService : GameInterfaceBTiService.GameInterfaceBTiServiceBase
{
    private readonly IGetService<BalanceQuery, BalanceResult> _balanceService;
    private readonly ICommandService<CreatePlayerCommand> _createPlayerService;
    private readonly IGetService<FundInQuery, FundResult> _fundInService;
    private readonly IGetService<FundOrderCheckingQuery, bool> _fundOrderCheckingService;
    private readonly IGetService<FundOutQuery, FundResult> _fundOutService;
    private readonly IQueryService<GameRecordQuery, GameRecordResult> _gameRecordService;

    public GameInterfaceBTiGrpcService(
         IGetService<BalanceQuery, BalanceResult> balanceService,
         ICommandService<CreatePlayerCommand> createPlayerService,
         IGetService<FundInQuery, FundResult> fundInService,
         IGetService<FundOrderCheckingQuery, bool> fundOrderCheckingService,
         IGetService<FundOutQuery, FundResult> fundOutService,
         IQueryService<GameRecordQuery, GameRecordResult> gameRecordService)
    {
        _balanceService = balanceService ?? throw new ArgumentNullException(nameof(balanceService));
        _createPlayerService = createPlayerService ?? throw new ArgumentNullException(nameof(createPlayerService));
        _fundInService = fundInService ?? throw new ArgumentNullException(nameof(fundInService));
        _fundOrderCheckingService = fundOrderCheckingService ?? throw new ArgumentNullException(nameof(fundOrderCheckingService));
        _fundOutService = fundOutService ?? throw new ArgumentNullException(nameof(fundOutService));
        _gameRecordService = gameRecordService ?? throw new ArgumentNullException(nameof(gameRecordService));
    }

    public override async Task<Reply> CreatePlayer(CreatePlayerRequest request, ServerCallContext context)
    {
        await _createPlayerService.ExecuteAsync(new CreatePlayerCommand(
            ApiUrl: new(request.ApiUrl),
            AgentUserName: request.AgentUserName,
            AgentPassword: request.AgentPassword,
            Brand: request.Brand,
            PlayerCode: request.PlayerCode,
            Currency: request.Currency)).ConfigureAwait(false);

        return new Reply()
        {
            Success = true
        };
    }

    public override async Task<FundReply> FundIn(FundRequest request, ServerCallContext context)
    {
        var result = await _fundInService.GetAsync(new FundInQuery(
             ApiUrl: new(request.ApiUrl),
             AgentUserName: request.AgentUserName,
             AgentPassword: request.AgentPassword,
             PlayerCode: request.PlayerCode,
             TransactionId: request.TransactionId,
             Amount: request.Amount)).ConfigureAwait(false);

        return new FundReply()
        {
            TransactionId = result.TransactionId,
            Balance = result.Balance,
        };
    }

    public override async Task<FundOrderCheckingReply> FundOrderChecking(FundOrderCheckingRequest request, ServerCallContext context)
    {
        var result = await _fundOrderCheckingService.GetAsync(new FundOrderCheckingQuery(
             ApiUrl: new(request.ApiUrl),
             AgentUserName: request.AgentUserName,
             AgentPassword: request.AgentPassword,
             TransactionId: request.TransactionId)).ConfigureAwait(false);

        return new FundOrderCheckingReply()
        {
            Exist = result
        };
    }

    public override async Task<FundReply> FundOut(FundRequest request, ServerCallContext context)
    {
        var result = await _fundOutService.GetAsync(new FundOutQuery(
             ApiUrl: new(request.ApiUrl),
             AgentUserName: request.AgentUserName,
             AgentPassword: request.AgentPassword,
             PlayerCode: request.PlayerCode,
             TransactionId: request.TransactionId,
             Amount: request.Amount)).ConfigureAwait(false);

        return new FundReply()
        {
            TransactionId = result.TransactionId,
            Balance = result.Balance,
        };
    }

    public override async Task<BalanceReply> GetBalance(BalanceRequest request, ServerCallContext context)
    {
        var result = await _balanceService.GetAsync(new BalanceQuery(
             ApiUrl: new(request.ApiUrl),
             AgentUserName: request.AgentUserName,
             AgentPassword: request.AgentPassword,
             PlayerCode: request.PlayerCode)).ConfigureAwait(false);

        return new BalanceReply()
        {
            IsPlayerExist = result.IsPlayerExist,
            Balance = result.Balance
        };
    }

    public override async Task<GamePullRecordReply> GetGamePullRecord(GamePullRecordRequest request, ServerCallContext context)
    {
        var result = await _gameRecordService.QueryAsync(new GameRecordQuery(
            ApiUrl: new(request.ApiUrl),
            AgentUserName: request.AgentUserName,
            AgentPassword: request.AgentPassword,
            StartTime: request.StartTime.ToDateTimeOffset(),
            EndTime: request.EndTime.ToDateTimeOffset(),
            Page: request.Page,
            PageLimit: request.PageLimit)).ToArrayAsync().ConfigureAwait(false);

        var reply = new GamePullRecordReply();

        if (result is null)
        {
            return reply;
        }

        foreach (var transaction in result)
        {
            reply.Items.Add(new GamePullRecord()
            {
                //Account = transaction.Account,
                //WagersId = transaction.WagersId,
                //GameId = transaction.GameId,
                //WagersTime = transaction.WagersTime.ToTimestamp(),
                //BetAmount = transaction.BetAmount,
                //PayoffTime = transaction.PayoffTime.ToTimestamp(),
                //PayoffAmount = transaction.PayoffAmount,
                //Status = (GamePullRecordStatus)transaction.Status,
                //SettlementTime = transaction.SettlementTime.ToTimestamp(),
                //GameCategoryId = transaction.GameCategoryId,
                //Type = transaction.Type,
                //AgentId = transaction.AgentId
            });
        };

        return reply;
    }
}
