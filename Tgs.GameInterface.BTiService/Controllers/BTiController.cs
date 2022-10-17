using Microsoft.AspNetCore.Mvc;
using Tgs.GameInterface.BTiService.Core.ApplicationServices;
using Tgs.GameInterface.BTiService.ViewModels;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.BTiService.Controllers;

[ApiController]
[Route("BTi")]
public class BTiController : ControllerBase
{
    [HttpPost("create")]
    public ValueTask CreatePlayerAsync(
        [FromServices] ICommandService<CreatePlayerCommand> service,
        CreatePlayerViewModel model)
        => service.ExecuteAsync(new CreatePlayerCommand(
            ApiUrl: model.ApiUrl,
            AgentUserName: model.AgentUserName,
            AgentPassword: model.AgentPassword,
            PlayerCode: model.PlayerCode,
            Currency: model.Currency));

    [HttpPost("fund-in")]
    public async ValueTask<FundResultViewModel> FundInAsync(
        [FromServices] IGetService<FundInQuery, FundInResult> service,
        FundQueryViewModel model)
    {
        var result = await service.GetAsync(new FundInQuery(
            ApiUrl: model.ApiUrl,
            AgentUserName: model.AgentUserName,
            AgentPassword: model.AgentPassword,
            PlayerCode: model.PlayerCode,
            TransactionId: model.TransactionId,
            Amount: model.Amount)).ConfigureAwait(false);

        return new FundResultViewModel(
            result.Success,
            result.TransactionId,
            result.BalanceAfter);
    }

    [HttpPost("fundorderchecking")]
    public ValueTask<bool> FundOrderCheckingAsync(
        [FromServices] IGetService<FundOrderCheckingQuery, bool> service,
        FundOrderCheckingViewModel model)
            => service.GetAsync(new FundOrderCheckingQuery(
                ApiUrl: model.ApiUrl,
                AgentUserName: model.AgentUserName,
                AgentPassword: model.AgentPassword,
                TransactionId: model.TransactionId));

    [HttpPost("fund-out")]
    public async ValueTask<FundResultViewModel> FundOutAsync(
        [FromServices] IGetService<FundOutQuery, FundOutResult> service,
        FundQueryViewModel model)
    {
        var result = await service.GetAsync(new FundOutQuery(
            ApiUrl: model.ApiUrl,
            AgentUserName: model.AgentUserName,
            AgentPassword: model.AgentPassword,
            PlayerCode: model.PlayerCode,
            TransactionId: model.TransactionId,
            Amount: model.Amount)).ConfigureAwait(false);

        return new FundResultViewModel(
            result.Success,
            result.TransactionId,
            result.BalanceAfter);
    }

    [HttpPost("gameurl")]
    public async ValueTask<GameUrlResultViewModel> GameUrlAsync(
        [FromServices] IGetService<GameUrlQuery, GameUrlResult> service,
        GameUrlQueryViewModel model)
    {
        var result = await service.GetAsync(new GameUrlQuery(
            ApiUrl: model.ApiUrl,
            AgentUserName: model.AgentUserName,
            AgentPassword: model.AgentPassword,
            PlayerCode: model.PlayerCode,
            ExternalGameId: model.ExternalGameId,
            Language: model.Language,
            Currency: model.Currency,
            IsMobile: model.IsMobile
            )).ConfigureAwait(false);

        return new GameUrlResultViewModel(
            Success: result.Success,
            Message: result.Message,
            IsPlayerExist: result.IsPlayerExist,
            GameUrl: result.GameUrl);
    }

    [HttpPost("balance")]
    public async ValueTask<BalanceResultViewModel> GetBalanceAsync(
        [FromServices] IGetService<BalanceQuery, BalanceResult> service,
        BalanceQueryViewModel model)
    {
        var result = await service.GetAsync(new BalanceQuery(
            ApiUrl: model.ApiUrl,
            AgentUserName: model.AgentUserName,
            AgentPassword: model.AgentPassword,
            PlayerCode: model.PlayerCode)).ConfigureAwait(false);

        return new BalanceResultViewModel(
            result.IsPlayerExist,
            result.Balance);
    }

    [HttpPost("game/pullrecord")]
    public async IAsyncEnumerable<GamePullRecordResultViewModel> GetGamePullRecordAsync(
            [FromServices] IQueryService<GameRecordQuery, GameRecordResult> service,
            GamePullRecordQueryViewModel model)
    {
        var result = await service.QueryAsync(new GameRecordQuery(
            ApiUrl: model.ApiUrl,
            model.AgentUserName,
            model.AgentPassword,
            StartTime: model.StartTime,
            EndTime: model.EndTime,
            Page: model.Page,
            PageLimit: model.PageLimit)).ToArrayAsync().ConfigureAwait(false);

        if (!result.Any())
        {
            yield break;
        }

        foreach (var transaction in result)
        {
            yield return new GamePullRecordResultViewModel(
                         //Account: transaction.Account,
                         //WagersId: transaction.WagersId,
                         //GameId: transaction.GameId,
                         //WagersTime: transaction.WagersTime,
                         //BetAmount: transaction.BetAmount,
                         //PayoffTime: transaction.PayoffTime,
                         //PayoffAmount: transaction.PayoffAmount,
                         //Status: (GameInterfaceJiliServiceGamePullRecordStatus)transaction.Status,
                         //SettlementTime: transaction.SettlementTime,
                         //GameCategoryId: transaction.GameCategoryId,
                         //Type: transaction.Type,
                         //AgentId: transaction.AgentId
                         );
        }
    }
}
