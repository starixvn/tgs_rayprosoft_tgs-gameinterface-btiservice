using System.Diagnostics;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Web;
using Microsoft.Extensions.Logging;
using Tgs.GameInterface.BTiService.Core.Exceptions;
using Tgs.GameInterface.BTiService.Core.Models;

namespace Tgs.GameInterface.BTiService.Core;

internal class GameDataApiClient : IGameDataApiClient
{
    private readonly HttpClient _client;
    private readonly ILogger<GameDataApiClient> _logger;
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public GameDataApiClient(
        HttpClient client,
        ILogger<GameDataApiClient> logger)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };
    }

    public async IAsyncEnumerable<BettingHistoryBet> BettingHistoryAsync(Uri apiUrl, string token, DateTime startTime, DateTime endTime, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        JsonObject request = new JsonObject
        {
            { "From", startTime.ToUniversalTime().ToString("s") },
            { "To", endTime.ToUniversalTime().ToString("s") }
        };

        var uriBuilder = new UriBuilder(apiUrl);
        var query = HttpUtility.ParseQueryString(uriBuilder.Query);
        query["token"] = token;
        uriBuilder.Query = query.ToString();

        var jsonNode = await PostAsync(
             uriBuilder.Uri,
            request,
            "Bettinghistory",
            cancellationToken).ConfigureAwait(false);

        if (jsonNode["errorCode"]?.GetValue<int>() is null or not 0)
        {
            throw new GameProviderResponseErrorException(jsonNode["errorCode"]?.ToString(), jsonNode["message"]?.ToString());
        }

        var bets = jsonNode["Bets"].Deserialize<IEnumerable<BettingHistoryBet>>(_jsonSerializerOptions) ?? Enumerable.Empty<BettingHistoryBet>();

        if (!bets.Any())
        {
            yield break;
        }

        foreach (var bet in bets)
        {
            yield return bet;
        }
        ;
    }

    public async IAsyncEnumerable<BetSettled> GetHistoryBetsPagingAsync(Uri apiUrl, string token, DateTimeOffset startTime, DateTimeOffset endTime, int page = 0, int pageLimit = 1000, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        JsonObject request = new JsonObject
        {
            { "From", startTime.ToUniversalTime().ToString("s") },
            { "To", endTime.ToUniversalTime().ToString("s") },
            { "Pagination", new JsonObject {
                {"page",page },
                {"rowperpage",pageLimit } }
            }
        };

        var url = new Uri(apiUrl, "/get-history-bets-paging");
        var uriBuilder = new UriBuilder(url);
        var query = HttpUtility.ParseQueryString(uriBuilder.Query);
        query["token"] = token;
        uriBuilder.Query = query.ToString();

        var jsonNode = await PostAsync(
            uriBuilder.Uri,
            request,
            "BettinghistoryPagination",
            cancellationToken).ConfigureAwait(false);

        if (jsonNode["errorCode"]?.GetValue<int>() is null or not 0)
        {
            throw new GameProviderResponseErrorException(jsonNode["errorCode"]?.ToString(), jsonNode["message"]?.ToString());
        }

        if (jsonNode["totalPages"]!.GetValue<int>() == -1)
        {
            yield break;
        }

        var bets = jsonNode["Bets"]?.AsArray();

        if (bets is null || bets.Count == 0)
        {
            yield break;
        }

        foreach (var bet in bets)
        {
            if (bet is null)
            {
                continue;
            }

            yield return new BetSettled(
                MerchantCustomerID: bet["MerchantCustomerID"]!.GetValue<string>(),
                PurchaseID: bet["PurchaseID"]!.GetValue<string>(),
                BetStatus: bet["BetStatus"]!.GetValue<string>(),
                CreationDate: bet["CreationDate"]!.GetValue<DateTimeOffset>(),
                BetSettledDate: bet["BetSettledDate"]!.GetValue<DateTimeOffset>(),
                TotalStake: bet["TotalStake"]!.GetValue<decimal>(),
                ValidStake: bet["ValidStake"]!.GetValue<decimal>(),
                PL: bet["PL"]!.GetValue<decimal>(),
                Return: bet["Return"]!.GetValue<decimal>(),
                AdditionalData: bet.ToJsonString());
        }
    }

    public async IAsyncEnumerable<BetUnsettled> GetOpenBetsPagingAsync(Uri apiUrl, string token, DateTimeOffset startTime, DateTimeOffset endTime, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        JsonObject request = new JsonObject
        {
            { "From", startTime.ToUniversalTime().ToString("s") },
            { "To", endTime.ToUniversalTime().ToString("s") }
        };

        var url = new Uri(apiUrl, "/get-open-bets-paging");
        var uriBuilder = new UriBuilder(url);
        var query = HttpUtility.ParseQueryString(uriBuilder.Query);
        query["token"] = token;
        uriBuilder.Query = query.ToString();

        var jsonNode = await PostAsync(
            uriBuilder.Uri,
            request,
            "OpenBets",
            cancellationToken).ConfigureAwait(false);

        if (jsonNode["errorCode"]?.GetValue<int>() is null or not 0)
        {
            throw new GameProviderResponseErrorException(jsonNode["errorCode"]?.ToString(), jsonNode["message"]?.ToString());
        }

        var bets = jsonNode["Bets"]?.AsArray();

        if (bets is null || bets.Count == 0)
        {
            yield break;
        }

        foreach (var bet in bets)
        {
            if (bet is null)
            {
                continue;
            }

            yield return new BetUnsettled(
                MerchantCustomerID: bet["MerchantCustomerID"]!.GetValue<string>(),
                PurchaseID: bet["PurchaseID"]!.GetValue<string>(),
                BetStatus: bet["BetStatus"]!.GetValue<string>(),
                CreationDate: bet["CreationDate"]!.GetValue<DateTimeOffset>(),
                TotalStake: bet["TotalStake"]!.GetValue<decimal>(),
                ValidStake: bet["ValidStake"]!.GetValue<decimal>(),
                AdditionalData: bet.ToJsonString());
        }
        ;
    }

    public async ValueTask<string> GetTokenAsync(Uri apiUrl, string agentUserName, string agentPassword, CancellationToken cancellationToken = default)
    {
        JsonObject request = new JsonObject
        {
            { "agentUserName", agentUserName },
            { "agentPassword", agentPassword }
        };

        var url = new Uri(apiUrl, "/authorize_v2");

        var jsonNode = await PostAsync(
            url,
            request,
            "GetToken",
            cancellationToken).ConfigureAwait(false);

        return jsonNode["errorCode"]?.GetValue<int>() is null or not 0
            ? throw new GameProviderResponseErrorException(jsonNode["errorCode"]?.ToString(), jsonNode["message"]?.ToString())
            : jsonNode["token"]!.ToString();
        ;
    }

    private async ValueTask<JsonNode> PostAsync(Uri apiUrl, JsonObject request, string logPrefix, CancellationToken cancellationToken = default)
    {
        logPrefix = $"{nameof(GameApiClient)}.{logPrefix}".ToLower();

        var sw = Stopwatch.StartNew();

        var content = new StringContent(request.ToJsonString(), Encoding.UTF8, MediaTypeNames.Application.Json);
        var httpResponseMessage = await _client.PostAsync(apiUrl, content, cancellationToken);

        sw.Stop();

        var responseMessage = default(string);

        using (var streamReader = new StreamReader(httpResponseMessage.Content.ReadAsStreamAsync(cancellationToken).Result))
        {
            responseMessage = await streamReader
                .ReadToEndAsync(
                    cancellationToken: cancellationToken)
                .ConfigureAwait(false) ?? string.Empty;
        }

        var apiLogEvent = new GameApiLogEvent
        {
            Path = httpResponseMessage.RequestMessage?.RequestUri?.AbsoluteUri.NoLongerThan(512) ?? string.Empty,
            Method = httpResponseMessage.RequestMessage?.Method.ToString().NoLongerThan(8) ?? string.Empty,
            Prefix = logPrefix,
            ServerName = Environment.MachineName.NoLongerThan(128),
            RequestHeaders = httpResponseMessage.RequestMessage?.Headers + httpResponseMessage.RequestMessage?.Content?.Headers?.ToString(),
            RequestContent = (await content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false)).NoLongerThan(4096),
            ResponseHeaders = httpResponseMessage.Headers.ToString(),
            ResponseContent = responseMessage.NoLongerThan(4096),
            ResponseCode = (short)httpResponseMessage.StatusCode,
            ExecutionTimeMs = (int)sw.ElapsedMilliseconds,
            RecordedOn = DateTimeOffset.UtcNow
        };

        _logger.LogInformation("{apiLogEvent}", JsonSerializer.Serialize(apiLogEvent));

        _ = httpResponseMessage.EnsureSuccessStatusCode();

        return JsonNode.Parse(responseMessage)!;
    }
}
