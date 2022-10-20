using Microsoft.AspNetCore.Mvc;
using Tgs.GameInterface.BTiService.Core.ApplicationServices;
using Tgs.GameInterface.BTiService.Protos;
using Tgs.GameInterface.BTiService.ViewModels;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.BTiService.Controllers;

[ApiController]
[Route("bti")]
public class BTiController : ControllerBase
{
    /// <summary>
    /// Funds the in asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "https://wallet.bti.bet/",
    ///             "agentUserName": "PegaGaming_GPIRMB",
    ///             "agentPassword": "IM60Ho7Y4B9C",
    ///             "merchantCustomerCode": "4ucmqplxb029",
    ///             "refTransactionCode": "4c640000e84770b57e0e08dab0e9face",
    ///             "amount": 1
    ///         }
    /// </remarks>
    [HttpPost("fund/in", Name = "FundIn")]
    public async ValueTask<FundResultViewModel> FundInAsync(
        [FromServices] IGetService<FundInQuery, FundResult> service,
        FundViewModel source)
    {
        var query = new FundInQuery(
            ApiUrl: source.ApiUrl,
            AgentUserName: source.AgentUserName,
            AgentPassword: source.AgentPassword,
            MerchantCustomerCode: source.MerchantCustomerCode,
            RefTransactionCode: source.RefTransactionCode,
            Amount: source.Amount);

        var result = await service
                .GetAsync(
                    query: query,
                    cancellationToken: HttpContext.RequestAborted)
                .ConfigureAwait(false);

        return new FundResultViewModel(
            RefTransactionCode: result.RefTransactionCode,
            TransactionId: result.TransactionId,
            Balance: result.Balance);
    }

    /// <summary>
    /// Funds the out asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "https://wallet.bti.bet/",
    ///             "agentUserName": "PegaGaming_GPIRMB",
    ///             "agentPassword": "IM60Ho7Y4B9C",
    ///             "merchantCustomerCode": "4ucmqplxb029",
    ///             "refTransactionCode": "fc730000e84770b5d8fc08dab0e9d6cd",
    ///             "amount": 1
    ///         }
    /// </remarks>
    [HttpPost("fund/out", Name = "FundOut")]
    public async ValueTask<FundResultViewModel> FundOutAsync(
            [FromServices] IGetService<FundOutQuery, FundResult> service,
            FundViewModel source)
    {
        var query = new FundOutQuery(
            ApiUrl: source.ApiUrl,
            AgentUserName: source.AgentUserName,
            AgentPassword: source.AgentPassword,
            MerchantCustomerCode: source.MerchantCustomerCode,
            RefTransactionCode: source.RefTransactionCode,
            Amount: source.Amount);

        var result = await service
                .GetAsync(
                    query: query,
                    cancellationToken: HttpContext.RequestAborted)
                .ConfigureAwait(false);

        return new FundResultViewModel(
            RefTransactionCode: result.RefTransactionCode,
            TransactionId: result.TransactionId,
            Balance: result.Balance);
    }

    /// <summary>
    /// Funds the status asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "https://wallet.bti.bet/",
    ///             "agentUserName": "PegaGaming_GPIRMB",
    ///             "agentPassword": "IM60Ho7Y4B9C",
    ///             "refTransactionCode": "4c640000e84770b57e0e08dab0e9face"
    ///         }
    /// </remarks>
    [HttpPost("fund/Status", Name = "FundStatus")]
    public ValueTask<bool> FundStatusAsync(
        [FromServices] IGetService<FundStatusQuery, bool> service,
        FundStatusViewModel source)
    {
        var query = new FundStatusQuery(
                ApiUrl: source.ApiUrl,
                AgentUserName: source.AgentUserName,
                AgentPassword: source.AgentPassword,
                RefTransactionCode: source.RefTransactionCode);

        return service.GetAsync(
                query: query,
                cancellationToken: HttpContext.RequestAborted);
    }

    /// <summary>
    /// Players the create asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "https://wallet.bti.bet/",
    ///             "agentUserName": "PegaGaming_GPIRMB",
    ///             "agentPassword": "IM60Ho7Y4B9C",
    ///             "brand": "GPI",
    ///             "merchantCustomerCode": "4ucmqplxb029",
    ///             "currency": "CNY"
    ///         }
    /// </remarks>
    [HttpPost("player/create", Name = "PlayerCreate")]
    public ValueTask PlayerCreateAsync(
        [FromServices] ICommandService<PlayerCreateCommand> service,
        PlayerCreateViewModel source)
    {
        var command = new PlayerCreateCommand(
            ApiUrl: source.ApiUrl,
            AgentUserName: source.AgentUserName,
            AgentPassword: source.AgentPassword,
            BrandCode: source.Brand,
            MerchantCustomerCode: source.MerchantCustomerCode,
            Currency: source.Currency);
        return service.ExecuteAsync(
                command: command,
                cancellationToken: HttpContext.RequestAborted);
    }

    /// <summary>
    /// Players the balance asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "https://wallet.bti.bet/",
    ///             "agentUserName": "PegaGaming_GPIRMB",
    ///             "agentPassword": "IM60Ho7Y4B9C",
    ///             "merchantCustomerCode": "4ucmqplxb029"
    ///         }
    /// </remarks>
    [HttpPost("player/balance", Name = "PlayerBalance")]
    public async ValueTask<PlayerBalanceResultViewModel> PlayerBalanceAsync(
        [FromServices] IGetService<PlayerBalanceQuery, PlayerBalanceResult> service,
        PlayerBalanceViewModel source)
    {
        var query = new PlayerBalanceQuery(
            ApiUrl: source.ApiUrl,
            AgentUserName: source.AgentUserName,
            AgentPassword: source.AgentPassword,
            MerchantCustomerCode: source.MerchantCustomerCode);

        var result = await service
                .GetAsync(
                    query: query,
                    cancellationToken: HttpContext.RequestAborted)
                .ConfigureAwait(false);

        return new PlayerBalanceResultViewModel(
            result.IsPlayerExist,
            result.Balance);
    }

    /// <summary>
    /// Games the authentication token asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="model">The model.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "https://wallet.bti.bet/",
    ///             "agentUserName": "PegaGaming_GPIRMB",
    ///             "agentPassword": "IM60Ho7Y4B9C",
    ///             "merchantCustomerCode": "4ucmqplxb029"
    ///         }
    /// </remarks>
    [HttpPost("game/authtoken", Name = "GameGetAuthToken")]
    public async ValueTask<GameAuthTokenResultViewModel> GameAuthTokenAsync(
            [FromServices] IGetService<GameAuthTokenQuery, GameAuthTokenResult> service,
            GameAuthTokenViewModel model)
    {
        var query = new GameAuthTokenQuery(
            ApiUrl: model.ApiUrl,
            model.AgentUserName,
            model.AgentPassword,
            MerchantCustomerCode: model.MerchantCustomerCode);

        var result = await service
                .GetAsync(
                    query: query,
                    cancellationToken: HttpContext.RequestAborted)
                .ConfigureAwait(false);

        return new GameAuthTokenResultViewModel(
            IsPlayerExist: result.IsPlayerExist,
            AuthToken: result.AuthToken);
    }

    /// <summary>
    /// Games the history settled asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="model">The model.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "tokenApiUrl": "https://asia-east2-bq-data-api.cloudfunctions.net/",
    ///             "historyBetsPagingApiUrl": "https://get-history-bets-paging-d5nlg5buca-de.a.run.app/",
    ///             "agentUserName": "PegaGaming_GPIRMB",
    ///             "agentPassword": "IM60Ho7Y4B9C",
    ///             "startTime": "2022-10-19T09:00:00+08:00",
    ///             "endTime": "2022-10-19T10:00:00+08:00",
    ///             "page": 0,
    ///             "pageLimit": 1000
    ///         }
    /// </remarks>
    [HttpPost("game/history/settled", Name = "GameHistorySettled")]
    public async IAsyncEnumerable<GameHistorySettledResultViewModel> GameHistorySettledAsync(
            [FromServices] IQueryService<GameHistorySettledQuery, GameHistorySettledResult> service,
            GameHistorySettledViewModel model)
    {
        var query = new GameHistorySettledQuery(
            TokenApiUrl: model.TokenApiUrl,
            HistoryBetsPagingApiUrl: model.HistoryBetsPagingApiUrl,
            model.AgentUserName,
            model.AgentPassword,
            StartTime: model.StartTime,
            EndTime: model.EndTime,
            Page: model.Page,
            PageLimit: model.PageLimit);

        var gameRecords = await service
                .QueryAsync(
                    query: query,
                    cancellationToken: HttpContext.RequestAborted)
                .ToArrayAsync(
                    cancellationToken: HttpContext.RequestAborted)
                .ConfigureAwait(false);

        if (!gameRecords.Any())
        {
            yield break;
        }

        foreach (var gameRecord in gameRecords)
        {
            yield return new GameHistorySettledResultViewModel(
                PlayerCode: gameRecord.PlayerCode,
                ExternalRoundId: gameRecord.ExternalRoundId,
                IsClosed: gameRecord.IsClosed,
                Status: (GameHistoryStatus)Enum.Parse(typeof(GameHistoryStatus), gameRecord.Status, true),
                ExternalCreatedOn: gameRecord.ExternalCreatedOn,
                ExternalClosedOn: gameRecord.ExternalClosedOn,
                Bet: gameRecord.Bet,
                ValidBet: gameRecord.ValidBet,
                PlayerWinLoss: gameRecord.PlayerWinLoss,
                Turnover: gameRecord.Turnover,
                Settle: gameRecord.Settle,
                AdditionalData: gameRecord.AdditionalData);
        }
    }

    /// <summary>
    /// Games the history unsettled asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="model">The model.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "tokenApiUrl": "https://asia-east2-bq-data-api.cloudfunctions.net/",
    ///             "openBetsApiUrl": "https://get-open-bets-d5nlg5buca-de.a.run.app/",
    ///             "agentUserName": "PegaGaming_GPIRMB",
    ///             "agentPassword": "IM60Ho7Y4B9C",
    ///             "startTime": "2022-10-19T09:00:00+08:00",
    ///             "endTime": "2022-10-19T10:00:00+08:00"
    ///         }
    /// </remarks>
    [HttpPost("game/history/unsettled", Name = "GameHistoryUnsettled")]
    public async IAsyncEnumerable<GameHistoryUnsettledResultViewModel> GameHistoryUnsettledAsync(
        [FromServices] IQueryService<GameHistoryUnsettledQuery, GameHistoryUnsettledResult> service,
        GameHistoryUnsettledViewModel model)
    {
        var query = new GameHistoryUnsettledQuery(
            TokenApiUrl: model.TokenApiUrl,
            OpenBetsApiUrl: model.OpenBetsApiUrl,
            model.AgentUserName,
            model.AgentPassword,
            StartTime: model.StartTime,
            EndTime: model.EndTime);

        var gameRecords = await service
                .QueryAsync(
                    query: query,
                    cancellationToken: HttpContext.RequestAborted)
                .ToArrayAsync(
                    cancellationToken: HttpContext.RequestAborted)
                .ConfigureAwait(false);

        if (!gameRecords.Any())
        {
            yield break;
        }

        foreach (var gameRecord in gameRecords)
        {
            yield return new GameHistoryUnsettledResultViewModel(
                PlayerCode: gameRecord.PlayerCode,
                ExternalRoundId: gameRecord.ExternalRoundId,
                ExternalCreatedOn: gameRecord.ExternalCreatedOn,
                Bet: gameRecord.Bet,
                ValidBet: gameRecord.ValidBet,
                AdditionalData: gameRecord.AdditionalData);
        }
    }
}
