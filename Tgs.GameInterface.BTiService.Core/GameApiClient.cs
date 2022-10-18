using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text.Json;
using System.Web;
using System.Xml;
using Microsoft.Extensions.Logging;
using Tgs.GameInterface.BTiService.Core.Exceptions;
using Tgs.GameInterface.BTiService.Core.Models;

namespace Tgs.GameInterface.BTiService.Core;

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

    public async ValueTask<bool> CheckTransactionAsync(
        Uri apiUrl,
        string agentUserName,
        string agentPassword,
        string refTransactionCode,
        CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<string, string>
        {
            ["AgentUserName"] = agentUserName,
            ["AgentPassword"] = agentPassword,
            ["RefTransactionCode"] = refTransactionCode
        };

        var url = new Uri(apiUrl, "/api/walletservice/WHLCustomers/CheckTransaction");

        var xmlDocument = await PostAsync(
            url,
            parameters,
            "CheckTransaction",
            cancellationToken).ConfigureAwait(false);

        var errorCode = xmlDocument.GetElementsByTagName("ErrorCode")[0]?.InnerText;

        if (errorCode is null
            || !errorCode.Equals("NoError", StringComparison.OrdinalIgnoreCase))
        {
            return errorCode is null || !errorCode.Equals("TransactionCodeNotFound", StringComparison.OrdinalIgnoreCase)
               ? throw new GameProviderResponseErrorException(errorCode)
               : false;
        }

        var transactionID = xmlDocument.GetElementsByTagName("TransactionID")[0]?.InnerText;

        return string.IsNullOrWhiteSpace(transactionID) ?
            throw new Exception("取得交易單號是空的") :
            true;
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

        var xmlDocument = await PostAsync(
            apiUrl,
            "CreateNewUser",
            parameters,
            "CreateNewUser",
            cancellationToken).ConfigureAwait(false);

        if (xmlDocument["ErrorCode"]?.ToString() is null or not "NoError")
        {
            throw new GameProviderResponseErrorException(xmlDocument["ErrorCode"]?.ToString());
        }

        //TODO: Redis Cache CustomerID
        var redisKey = playerCode;
        var customerID = int.Parse(xmlDocument["CustomerID"]?.ToString() ?? throw new Exception());
    }

    public async ValueTask<GetCustomerAuthTokenResult> GetCustomerAuthTokenAsync(
        Uri apiUrl, 
        string agentUserName,
        string agentPassword, 
        string merchantCustomerCode, 
        CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<string, string>
        {
            ["AgentUserName"] = agentUserName,
            ["AgentPassword"] = agentPassword,
            ["MerchantCustomerCode"] = merchantCustomerCode,
        };

        var xmlDocument = await PostAsync(
            apiUrl,
            "GetCustomerAuthToken",
            parameters,
            "GetCustomerAuthToken",
            cancellationToken).ConfigureAwait(false);

        if (xmlDocument["ErrorCode"]?.ToString() is null or not "NoError")
        {
            throw new GameProviderResponseErrorException(xmlDocument["ErrorCode"]?.ToString());
        }

        return new GetCustomerAuthTokenResult(xmlDocument["AuthToken"]?.ToString() ?? throw new Exception(""));
    }

    public async ValueTask<GetCustomerByMerchantCodeResult> GetCustomerByMerchantCodeAsync(
        Uri apiUrl, 
        string agentUserName, 
        string agentPassword, 
        string merchantCustomerCode, 
        CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<string, string>
        {
            ["AgentUserName"] = agentUserName,
            ["AgentPassword"] = agentPassword,
            ["MerchantCustomerCode"] = merchantCustomerCode,
        };

        var url = new Uri(apiUrl, "/api/walletservice/WHLCustomers/GetCustomerByMerchantCode");

        var xmlDocument = await PostAsync(
            url,
            parameters,
            "GetCustomerByMerchantCode",
            cancellationToken).ConfigureAwait(false);

        var errorCode = xmlDocument.GetElementsByTagName("ErrorCode")[0]?.InnerText;

        if (errorCode is null
            || !errorCode.Equals("NoError", StringComparison.OrdinalIgnoreCase))
        {
            return errorCode is null || !errorCode.Equals("CustomerNotFound", StringComparison.OrdinalIgnoreCase)
                ? throw new GameProviderResponseErrorException(errorCode)
                : new GetCustomerByMerchantCodeResult(
                    IsPlayerExist: false,
                    Balance: 0);
        }

        var balance = decimal.Parse(xmlDocument.GetElementsByTagName("Balance")[0]?.InnerText!);

        return new GetCustomerByMerchantCodeResult(
                    IsPlayerExist: true,
                    Balance: balance);
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

    public async ValueTask<TransferWHLResult> TransferFromWHLAsync(
        Uri apiUrl,
        string agentUserName,
        string agentPassword,
        string merchantCustomerCode,
        string refTransactionCode,
        decimal amount,
        CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<string, string>
        {
            ["AgentUserName"] = agentUserName,
            ["AgentPassword"] = agentPassword,
            ["MerchantCustomerCode"] = merchantCustomerCode,
            ["RefTransactionCode"] = refTransactionCode,
            ["Amount"] = amount.ToString()
        };

        var url = new Uri(apiUrl, "/api/walletservice/WHLCustomers/TransferFromWHL");

        var xmlDocument = await PostAsync(
            url,
            parameters,
            "TransferFromWHL",
            cancellationToken).ConfigureAwait(false);

        var errorCode = xmlDocument.GetElementsByTagName("ErrorCode")[0]?.InnerText;

        if (errorCode is null
            || !errorCode.Equals("NoError", StringComparison.OrdinalIgnoreCase))
        {
            throw new GameProviderResponseErrorException(errorCode);
        }

        var balance = decimal.Parse(xmlDocument.GetElementsByTagName("Balance")[0]?.InnerText ?? throw new Exception());
        var transactionID = xmlDocument.GetElementsByTagName("TransactionID")[0]?.InnerText ?? throw new Exception("取得交易單號是空的");

        return new TransferWHLResult(refTransactionCode, transactionID, balance);
    }

    public async ValueTask<TransferWHLResult> TransferToWHLAsync(
        Uri apiUrl,
        string agentUserName,
        string agentPassword,
        string merchantCustomerCode,
        string refTransactionCode,
        decimal amount,
        CancellationToken cancellationToken = default)
    {
        var parameters = new Dictionary<string, string>
        {
            ["AgentUserName"] = agentUserName,
            ["AgentPassword"] = agentPassword,
            ["MerchantCustomerCode"] = merchantCustomerCode,
            ["RefTransactionCode"] = refTransactionCode,
            ["Amount"] = amount.ToString()
        };

        var url = new Uri(apiUrl, "/api/walletservice/WHLCustomers/TransferToWHL");

        var xmlDocument = await PostAsync(
            url,
            parameters,
            "TransferToWHL",
            cancellationToken).ConfigureAwait(false);

        var errorCode = xmlDocument.GetElementsByTagName("ErrorCode")[0]?.InnerText;

        if (errorCode is null
            || !errorCode.Equals("NoError", StringComparison.OrdinalIgnoreCase))
        {
            throw new GameProviderResponseErrorException(errorCode);
        }

        var balance = decimal.Parse(xmlDocument.GetElementsByTagName("Balance")[0]?.InnerText ?? throw new Exception());
        var transactionID = xmlDocument.GetElementsByTagName("TransactionID")[0]?.InnerText ?? throw new Exception("取得交易單號是空的");

        return new TransferWHLResult(refTransactionCode, transactionID, balance);
    }

    private async ValueTask<XmlDocument> PostAsync(Uri apiUrl, string path, IEnumerable<KeyValuePair<string, string>> parameters, string logPrefix, CancellationToken cancellationToken = default)
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

        var xmlDoc = new XmlDocument();
        xmlDoc.Load(responseMessage);
        return xmlDoc;
    }

    private async ValueTask<XmlDocument> PostAsync(Uri apiUrl, IEnumerable<KeyValuePair<string, string>> parameters, string logPrefix, CancellationToken cancellationToken = default)
    {
        logPrefix = $"{nameof(GameApiClient)}.{logPrefix}".ToLower();

        var sw = Stopwatch.StartNew();

        HttpContent content = new FormUrlEncodedContent(parameters);

        var httpResponseMessage = await _client.PostAsync(apiUrl, content, cancellationToken);

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

        var xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(responseMessage);
        return xmlDoc;
    }
}
