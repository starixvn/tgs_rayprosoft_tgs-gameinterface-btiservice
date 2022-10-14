using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text.Json;
using System.Web;
using Microsoft.Extensions.Logging;
using Tgs.GameInterface.BTiService.Core;
using Tgs.GameInterface.BTiService.Core.Models;
using Tgs.GameInterface.BTiService.Repositories.Models;

namespace Tgs.GameInterface.BTiService.Repositories;

internal class GameApiClient : IGameApiClient
{
    private readonly HttpClient _client;
    private readonly ILogger<GameApiClient> _logger;

    public GameApiClient(
        HttpClient client,
        ILogger<GameApiClient> logger)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async ValueTask<BettinghistoryResult> BettinghistoryAsync(
        Uri apiUrl,
        string agentUserName,
        string agentPassword,
        DateTime startTime,
        DateTime endTime,
        CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<string, string>
        {
            ["From"] = startTime.ToString("yyyy-MM-ddThh:mm:ss"),
            ["To"] = endTime.ToString("yyyy-MM-ddThh:mm:ss")
        };

        var token = await GetTokenAsync(apiUrl, agentUserName, agentPassword, cancellationToken);
        var url = new Uri(apiUrl, $"?token={token}");

        var document = await PostAsync(
            url,
            "Bettinghistory",
            parameters,
            "Bettinghistory",
            cancellationToken).ConfigureAwait(false);

        if (document is null)
        {
            throw new Exception("Document is empty.");
        }

        if (!document.RootElement.TryGetProperty("ErrorCode", out var jsonErrorCode))
        {
            throw new Exception($"JsonElement parsing failed.({JsonSerializer.Serialize(document)})");
        }

        var errorCode = jsonErrorCode.ToString();

        if (errorCode is not "NoError")
        {
            throw new Exception($"Failed to get betting history.({startTime:yyyy-MM-dd HH:mm:ss} - {endTime:yyyy-MM-dd HH:mm:ss})");
        }

        var jsonRootResult = document.RootElement.GetRawText();

        var betHistoryModel = JsonSerializer.Deserialize<BettingHistoryViewModel>(jsonRootResult);

        return new BettinghistoryResult(true, betHistoryModel);
    }

    public async ValueTask<BettinghistoryResult> BettinghistoryPaginationAsync(Uri apiUrl, string agentUserName, string agentPassword, DateTime startTime, DateTime endTime, int page = 0, int pageLimit = 1000, CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<string, string>
        {
            ["From"] = startTime.ToString("yyyy-MM-ddThh:mm:ss"),
            ["To"] = endTime.ToString("yyyy-MM-ddThh:mm:ss"),
            ["pagination"] = JsonSerializer.Serialize(new { page = page, rowperpage = pageLimit })
        };

        var token = await GetTokenAsync(apiUrl, agentUserName, agentPassword, cancellationToken);
        var url = new Uri(apiUrl, $"?token={token}");

        var document = await PostAsync(
            url,
            "BettinghistoryPagination",
            parameters,
            "BettinghistoryPagination",
            cancellationToken).ConfigureAwait(false);

        if (document is null)
        {
            throw new Exception("Document is empty.");
        }

        if (!document.RootElement.TryGetProperty("ErrorCode", out var jsonErrorCode))
        {
            throw new Exception($"JsonElement parsing failed.({JsonSerializer.Serialize(document)})");
        }

        var errorCode = jsonErrorCode.ToString();

        if (errorCode is not "NoError")
        {
            throw new Exception($"Failed to get betting history.({startTime:yyyy-MM-dd HH:mm:ss} - {endTime:yyyy-MM-dd HH:mm:ss})");
        }

        return new BettinghistoryResult(true, null);
    }

    public async ValueTask<bool> CheckTransactionAsync(Uri apiUrl, string agentUserName, string agentPassword, string transactionId, CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<string, string>
        {
            ["AgentUserName"] = agentUserName,
            ["AgentPassword"] = agentPassword,
            ["RefTransactionCode"] = transactionId,
        };

        var document = await PostAsync(
            apiUrl,
            "CheckTransaction",
            parameters,
            "CheckTransaction",
            cancellationToken).ConfigureAwait(false);

        if (document is null)
        {
            throw new Exception("Document is empty.");
        }

        if (!document.RootElement.TryGetProperty("ErrorCode", out var jsonErrorCode))
        {
            throw new Exception($"JsonElement parsing failed.({JsonSerializer.Serialize(document)})");
        }

        var errorCode = jsonErrorCode.ToString();

        if (errorCode is not "NoError")
        {
            return false;
        }

        _ = document.RootElement.TryGetProperty("TransactionID", out var jsonTransactionID);

        var transactionID = jsonTransactionID.GetInt32();

        return true;
    }

    public async ValueTask CreateNewUserAsync(Uri apiUrl, string agentUserName, string agentPassword, string playerCode, string currencyCode, string countryCode, string customerDefaultLanguage, CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<string, string>
        {
            ["AgentUserName"] = agentUserName,
            ["AgentPassword"] = agentPassword,
            ["MerchantCustomerCode"] = playerCode,
            ["LoginName"] = playerCode,
            ["CurrencyCode"] = currencyCode,
            ["CountryCode"] = countryCode,
            ["City"] = "",
            ["FirstName"] = "",
            ["LastName"] = "",
            ["Group1ID"] = 0.ToString(),
            ["CustomerMoreInfo"] = "",
            ["CustomerDefaultLanguage"] = "",
            ["DomainID"] = "",
            ["DateOfBirth"] = "",
        };

        var document = await PostAsync(
            apiUrl,
            "CreateNewUser",
            parameters,
            "CreateNewUser",
            cancellationToken).ConfigureAwait(false);

        if (document is null)
        {
            throw new Exception("Document is empty.");
        }

        if (!document.RootElement.TryGetProperty("ErrorCode", out var jsonErrorCode))
        {
            throw new Exception($"JsonElement parsing failed.({JsonSerializer.Serialize(document)})");
        }

        var errorCode = jsonErrorCode.ToString();

        if (errorCode is not "NoError")
        {
            throw new Exception($"Failed to create player.({playerCode})");
        }

        _ = document.RootElement.TryGetProperty("CustomerID", out var customerID);
        //TODO: Redis Cache CustomerID
        var redisKey = playerCode;
        var redisValue = customerID.GetInt32();
    }

    public async ValueTask<GetBalanceResult> GetBalanceAsync(Uri apiUrl, string agentUserName, string agentPassword, string playerCode, CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<string, string>
        {
            ["AgentUserName"] = agentUserName,
            ["AgentPassword"] = agentPassword,
            ["MerchantCustomerCode"] = playerCode,
        };

        var document = await PostAsync(
            apiUrl,
            "GetBalance",
            parameters,
            "GetBalance",
            cancellationToken).ConfigureAwait(false);

        if (document is null)
        {
            throw new Exception("Document is empty.");
        }

        if (!document.RootElement.TryGetProperty("ErrorCode", out var jsonErrorCode))
        {
            throw new Exception($"JsonElement parsing failed.({JsonSerializer.Serialize(document)})");
        }

        var errorCode = jsonErrorCode.ToString();

        if (errorCode is not "NoError")
        {
            throw new Exception($"Failed to get balance.({playerCode})");
        }

        _ = document.RootElement.TryGetProperty("Balance", out var jsonBalance);

        var balance = jsonBalance.GetDecimal();

        return new GetBalanceResult(true, true, balance);
    }

    public async ValueTask<GetCustomerAuthTokenResult> GetCustomerAuthTokenAsync(Uri apiUrl, string agentUserName, string agentPassword, string playerCode, CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<string, string>
        {
            ["AgentUserName"] = agentUserName,
            ["AgentPassword"] = agentPassword,
            ["MerchantCustomerCode"] = playerCode,
        };

        var document = await PostAsync(
            apiUrl,
            "GetCustomerAuthToken",
            parameters,
            "GetCustomerAuthToken",
            cancellationToken).ConfigureAwait(false);

        if (document is null)
        {
            throw new Exception("Document is empty.");
        }

        if (!document.RootElement.TryGetProperty("ErrorCode", out var jsonErrorCode))
        {
            throw new Exception($"JsonElement parsing failed.({JsonSerializer.Serialize(document)})");
        }

        var errorCode = jsonErrorCode.ToString();

        if (errorCode is not "NoError")
        {
            throw new Exception($"Failed to get customer auth token.({playerCode})");
        }

        document.RootElement.TryGetProperty("AuthToken", out var jsonAuthToken);

        var authToken = jsonAuthToken.ToString();

        return new GetCustomerAuthTokenResult(authToken);
    }

    public async ValueTask<GetCustomerByMerchantCodeResult> GetCustomerByMerchantCodeAsync(Uri apiUrl, string agentUserName, string agentPassword, string playerCode, CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<string, string>
        {
            ["AgentUserName"] = agentUserName,
            ["AgentPassword"] = agentPassword,
            ["MerchantCustomerCode"] = playerCode,
        };

        var document = await PostAsync(
            apiUrl,
            "GetCustomerByMerchantCode",
            parameters,
            "GetCustomerByMerchantCode",
            cancellationToken).ConfigureAwait(false);

        if (document is null)
        {
            throw new Exception("Document is empty.");
        }

        if (!document.RootElement.TryGetProperty("ErrorCode", out var jsonErrorCode))
        {
            throw new Exception($"JsonElement parsing failed.({JsonSerializer.Serialize(document)})");
        }

        var errorCode = jsonErrorCode.ToString();

        if (errorCode is not "NoError")
        {
            _logger.LogError("Failed to get customer data.({playerCode})", playerCode);
            return new GetCustomerByMerchantCodeResult(false);
        }

        _ = document.RootElement.TryGetProperty("Balance", out var jsonBalance);
        _ = document.RootElement.TryGetProperty("CustomerID", out var jsonCustomerID);

        var balance = jsonBalance.GetDecimal();
        var customerID = jsonCustomerID.GetInt32();

        return new GetCustomerByMerchantCodeResult(true, customerID, balance);
    }

    public ValueTask<GetGameUrlResult> GetGameUrlAsync(Uri apiUrl, string operatorToken, bool isMobile, string? oddsStyleId, string? sportId, string? leagueId, string? eventId, string? api, string language = "en", CancellationToken cancellationToken = default)
    {
        var view = "asian-view";
        if (isMobile)
            view = "sports";

        var queryStrings = HttpUtility.ParseQueryString(string.Empty);

        queryStrings.Add("operatorToken", operatorToken);

        if (oddsStyleId != null)
            queryStrings.Add("oddsStyleId", oddsStyleId);

        if (sportId != null)
            queryStrings.Add("sportId", sportId);

        if (leagueId != null)
            queryStrings.Add("leagueId", leagueId);

        if (eventId != null)
            queryStrings.Add("eventId", eventId);

        if (api != null)
            queryStrings.Add("api", api);

        var relativeUri = $"/{language}/{view}/?{queryStrings}";

        var url = new Uri(apiUrl, relativeUri);

        return ValueTask.FromResult(new GetGameUrlResult
        {
            Url = url.ToString(),
        });
    }

    public async ValueTask<string> GetTokenAsync(Uri apiUrl, string agentUserName, string agentPassword, CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<string, string>
        {
            ["AgentUserName"] = agentUserName,
            ["AgentPassword"] = agentPassword,
        };

        var document = await PostAsync(
            apiUrl,
            "GetToken",
            parameters,
            "GetToken",
            cancellationToken).ConfigureAwait(false);

        if (document is null)
        {
            throw new Exception("Document is empty.");
        }

        if (!document.RootElement.TryGetProperty("ErrorCode", out var jsonErrorCode))
        {
            throw new Exception($"JsonElement parsing failed.({JsonSerializer.Serialize(document)})");
        }

        var errorCode = jsonErrorCode.GetInt32();

        if (errorCode is not 0)
        {
            throw new Exception($"Failed to get token. AgentUserName:({agentUserName}), AgentPassword:({agentPassword})");
        }

        _ = document.RootElement.TryGetProperty("token", out var jsonToken);

        var token = jsonToken.ToString();

        return token;
    }

    public async ValueTask<TransferFromWHLResult> TransferFromWHLAsync(Uri apiUrl, string agentUserName, string agentPassword, string playerCode, decimal amount, string transactionId, CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<string, string>
        {
            ["AgentUserName"] = agentUserName,
            ["AgentPassword"] = agentPassword,
            ["MerchantCustomerCode"] = playerCode,
            ["Amount"] = amount.ToString(),
            ["RefTransactionCode"] = transactionId,
        };

        var document = await PostAsync(
            apiUrl,
            "TransferFromWHL",
            parameters,
            "TransferFromWHL",
            cancellationToken).ConfigureAwait(false);

        if (document is null)
        {
            throw new Exception("Document is empty.");
        }

        if (!document.RootElement.TryGetProperty("ErrorCode", out var jsonErrorCode))
        {
            throw new Exception($"JsonElement parsing failed.({JsonSerializer.Serialize(document)})");
        }

        var errorCode = jsonErrorCode.ToString();

        if (errorCode is not "NoError")
        {
            throw new Exception($"Failed to transfer amount.({playerCode})");
        }

        _ = document.RootElement.TryGetProperty("Balance", out var jsonBalance);
        _ = document.RootElement.TryGetProperty("TransactionID", out var jsonTransactionID);

        var balance = jsonBalance.GetDecimal();
        var transactionID = jsonTransactionID.GetInt32();

        return new TransferFromWHLResult(true, transactionID, balance);
    }

    public async ValueTask<TransferToWHLResult> TransferToWHLAsync(Uri apiUrl, string agentUserName, string agentPassword, string playerCode, decimal amount, string transactionId, CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<string, string>
        {
            ["AgentUserName"] = agentUserName,
            ["AgentPassword"] = agentPassword,
            ["MerchantCustomerCode"] = playerCode,
            ["Amount"] = amount.ToString(),
            ["RefTransactionCode"] = transactionId,
            ["BonusCode"] = "",
        };

        var document = await PostAsync(
            apiUrl,
            "TransferToWHL",
            parameters,
            "TransferToWHL",
            cancellationToken).ConfigureAwait(false);

        if (document is null)
        {
            throw new Exception("Document is empty.");
        }

        if (!document.RootElement.TryGetProperty("ErrorCode", out var jsonErrorCode))
        {
            throw new Exception($"JsonElement parsing failed.({JsonSerializer.Serialize(document)})");
        }

        var errorCode = jsonErrorCode.ToString();

        if (errorCode is not "NoError")
        {
            throw new Exception($"Failed to transfer amount.({playerCode})");
        }

        _ = document.RootElement.TryGetProperty("Balance", out var jsonBalance);
        _ = document.RootElement.TryGetProperty("TransactionID", out var jsonTransactionID);

        var balance = jsonBalance.GetDecimal();
        var transactionID = jsonTransactionID.GetInt32();

        return new TransferToWHLResult(true, transactionID, balance);
    }

    private async ValueTask<JsonDocument?> PostAsync(Uri apiUrl, string path, IEnumerable<KeyValuePair<string, string>> parameters, string logPrefix, CancellationToken cancellationToken = default)
    {
        logPrefix = $"{nameof(GameApiClient)}.{logPrefix}".ToLower();

        var sw = Stopwatch.StartNew();
        var url = new Uri(apiUrl, path);

        HttpContent content = new FormUrlEncodedContent(parameters);
        content.Headers.ContentType = new MediaTypeHeaderValue(MediaTypeNames.Application.Json)
        {
            CharSet = "UTF-8"
        };

        var httpResponseMessage = await _client.PostAsync(url, content, cancellationToken);

        sw.Stop();

        var responseMessage = default(string);

        using (var sr = new StreamReader(httpResponseMessage.Content.ReadAsStreamAsync(cancellationToken).Result))
        {
            responseMessage = await sr.ReadToEndAsync().ConfigureAwait(false) ?? string.Empty;
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

        return JsonDocument.Parse(responseMessage);
    }
}
