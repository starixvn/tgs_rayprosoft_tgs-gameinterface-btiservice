using System.Diagnostics;
using System.Xml;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Tgs.GameInterface.BTiService.Core.Exceptions;
using Tgs.GameInterface.BTiService.Core.Models;

namespace Tgs.GameInterface.BTiService.Core;

internal class GameApiClient(
	HttpClient client,
	ILogger<GameApiClient> logger) : IGameApiClient
{
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

	public async ValueTask CreateNewUserAsync(
		Uri apiUrl,
		string agentUserName,
		string agentPassword,
		string merchantCustomerCode,
		string currencyCode,
		string firstName,
		string lastName,
		string countryCode = "CN",
		string city = "Beijing",
		string customerDefaultLanguage = "zh",
		CancellationToken cancellationToken = default)
	{
		var parameters = new Dictionary<string, string>
		{
			["AgentUserName"] = agentUserName,
			["AgentPassword"] = agentPassword,
			["MerchantCustomerCode"] = merchantCustomerCode,
			["LoginName"] = merchantCustomerCode,
			["CurrencyCode"] = currencyCode,
			["CountryCode"] = countryCode,
			["City"] = city,
			["FirstName"] = firstName,
			["LastName"] = lastName,
			["Group1ID"] = "1",
			["CustomerMoreInfo"] = string.Empty,
			["CustomerDefaultLanguage"] = customerDefaultLanguage,
			["DomainID"] = string.Empty,
			["DateOfBirth"] = string.Empty,
		};

		var url = new Uri(apiUrl, "/api/walletservice/WHLCustomers/CreateUserNew");

		var xmlDocument = await PostAsync(
			url,
			parameters,
			"CreateUserNew",
			cancellationToken).ConfigureAwait(false);

		var errorCode = xmlDocument.GetElementsByTagName("ErrorCode")[0]?.InnerText;

		if (errorCode is null
			|| !errorCode.Equals("NoError", StringComparison.OrdinalIgnoreCase))
		{
			if (errorCode is null || !errorCode.Equals("DuplicateMerchantCustomerCode", StringComparison.OrdinalIgnoreCase))
			{
				throw new GameProviderResponseErrorException(errorCode);
			}
		}
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

		var url = new Uri(apiUrl, "/api/walletservice/WHLCustomers/GetCustomerAuthToken");

		var xmlDocument = await PostAsync(
			url,
			parameters,
			"GetCustomerAuthToken",
			cancellationToken).ConfigureAwait(false);

		var errorCode = xmlDocument.GetElementsByTagName("ErrorCode")[0]?.InnerText;

		if (errorCode is null
			|| !errorCode.Equals("NoError", StringComparison.OrdinalIgnoreCase))
		{
			return errorCode is null || !errorCode.Equals("CustomerNotFound", StringComparison.OrdinalIgnoreCase)
				? throw new GameProviderResponseErrorException(errorCode)
				: new GetCustomerAuthTokenResult(
					IsPlayerExist: false,
					AuthToken: string.Empty);
		}

		var authToken = xmlDocument.GetElementsByTagName("AuthToken")[0]?.InnerText!;

		return new GetCustomerAuthTokenResult(
					IsPlayerExist: true,
					AuthToken: authToken);
	}

	public async ValueTask<GetBalanceResult> GetBalanceAsync(
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

		var url = new Uri(apiUrl, "/api/walletservice/WHLCustomers/GetBalance");

		var xmlDocument = await PostAsync(
			url,
			parameters,
			"GetBalance",
			cancellationToken).ConfigureAwait(false);

		var errorCode = xmlDocument.GetElementsByTagName("ErrorCode")[0]?.InnerText;

		if (errorCode is null
			|| !errorCode.Equals("NoError", StringComparison.OrdinalIgnoreCase))
		{
			return errorCode is null || !errorCode.Equals("CustomerNotFound", StringComparison.OrdinalIgnoreCase)
				? throw new GameProviderResponseErrorException(errorCode)
				: new GetBalanceResult(
					IsPlayerExist: false,
					Balance: 0);
		}

		var balance = decimal.Parse(xmlDocument.GetElementsByTagName("Balance")[0]?.InnerText!);

		return new GetBalanceResult(
					IsPlayerExist: true,
					Balance: balance);
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

		return new TransferWHLResult(refTransactionCode, transactionID, balance, JsonConvert.SerializeXmlNode(xmlDocument));
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

		return new TransferWHLResult(refTransactionCode, transactionID, balance, JsonConvert.SerializeXmlNode(xmlDocument));
	}

	private async ValueTask<XmlDocument> PostAsync(Uri apiUrl, IEnumerable<KeyValuePair<string, string>> parameters, string logPrefix, CancellationToken cancellationToken = default)
	{
		logPrefix = $"{nameof(GameApiClient)}.{logPrefix}".ToLower();

		var sw = Stopwatch.StartNew();

		HttpContent content = new FormUrlEncodedContent(parameters);

		var httpResponseMessage = await client.PostAsync(apiUrl, content, cancellationToken).ConfigureAwait(false);

		sw.Stop();

		var responseMessage = default(string);

		using (var stream = new StreamReader(httpResponseMessage.Content.ReadAsStreamAsync(cancellationToken).Result))
		{
			responseMessage = await stream
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

		logger.LogInformation("{apiLogEvent}", System.Text.Json.JsonSerializer.Serialize(apiLogEvent));

		_ = httpResponseMessage.EnsureSuccessStatusCode();

		var xmlDoc = new XmlDocument();
		xmlDoc.LoadXml(responseMessage);
		return xmlDoc;
	}
}
