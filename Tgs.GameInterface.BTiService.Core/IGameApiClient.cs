using Tgs.GameInterface.BTiService.Core.Models;

namespace Tgs.GameInterface.BTiService.Core;

public interface IGameApiClient
{
    ValueTask<bool> CheckTransactionAsync(
        Uri apiUrl,
        string agentUserName,
        string agentPassword,
        string transactionId,
        CancellationToken cancellationToken = default);

    ValueTask CreateNewUserAsync(
        Uri apiUrl,
        string agentUserName,
        string agentPassword,
        string playerCode,
        string currencyCode,
        string countryCode,
        string customerDefaultLanguage,
        CancellationToken cancellationToken = default);

    ValueTask<GetBalanceResult> GetBalanceAsync(
        Uri apiUrl,
        string agentUserName,
        string agentPassword,
        string playerCode,
        CancellationToken cancellationToken = default);

    ValueTask<GetCustomerAuthTokenResult> GetCustomerAuthTokenAsync(
        Uri apiUrl,
        string agentUserName,
        string agentPassword,
        string playerCode,
        CancellationToken cancellationToken = default);

    ValueTask<GetCustomerByMerchantCodeResult> GetCustomerByMerchantCodeAsync(
        Uri apiUrl,
        string agentUserName,
        string agentPassword,
        string playerCode,
        CancellationToken cancellationToken = default);

    ValueTask<GetGameUrlResult> GetGameUrlAsync(Uri apiUrl, string operatorToken, bool isMobile, string? oddsStyleId, string? sportId, string? leagueId, string? eventId, string? api, string language = "en", CancellationToken cancellationToken = default);

    /// <summary>
    /// (GP -> Tgs) Transfers from WHL asynchronous.
    /// </summary>
    /// <param name="apiUrl">The API URL.</param>
    /// <param name="agentUserName">Name of the agent user.</param>
    /// <param name="agentPassword">The agent password.</param>
    /// <param name="playerCode">The player code.</param>
    /// <param name="amount">The amount.</param>
    /// <param name="transactionId">The transaction identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    ValueTask<TransferFromWHLResult> TransferFromWHLAsync(Uri apiUrl, string agentUserName, string agentPassword, string playerCode, decimal amount, string transactionId, CancellationToken cancellationToken = default);

    /// <summary>
    /// (Tgs -> GP) Transfers to WHL.
    /// </summary>
    /// <param name="apiUrl">The API URL.</param>
    /// <param name="agentUserName">Name of the agent user.</param>
    /// <param name="agentPassword">The agent password.</param>
    /// <param name="playerCode">The player code.</param>
    /// <param name="amount">The amount.</param>
    /// <param name="transactionId">The transaction identifier.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    ValueTask<TransferToWHLResult> TransferToWHLAsync(Uri apiUrl, string agentUserName, string agentPassword, string playerCode, decimal amount, string transactionId, CancellationToken cancellationToken = default);
}
