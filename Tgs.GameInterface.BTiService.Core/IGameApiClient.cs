using Tgs.GameInterface.BTiService.Core.Models;

namespace Tgs.GameInterface.BTiService.Core;

public interface IGameApiClient
{
    ValueTask<bool> CheckTransactionAsync(
        Uri apiUrl,
        string agentUserName,
        string agentPassword,
        string refTransactionCode,
        CancellationToken cancellationToken = default);

    ValueTask CreateNewUserAsync(
        Uri apiUrl,
        string agentUserName,
        string agentPassword,
        string merchantCustomerCode,
        string currencyCode,
        string countryCode,
        string customerDefaultLanguage,
        CancellationToken cancellationToken = default);

    ValueTask<GetCustomerAuthTokenResult> GetCustomerAuthTokenAsync(
        Uri apiUrl,
        string agentUserName,
        string agentPassword,
        string merchantCustomerCode,
        CancellationToken cancellationToken = default);

    ValueTask<GetBalanceResult> GetBalanceAsync(
        Uri apiUrl,
        string agentUserName,
        string agentPassword,
        string merchantCustomerCode,
        CancellationToken cancellationToken = default);

    ValueTask<GetGameUrlResult> GetGameUrlAsync(Uri apiUrl, string operatorToken, bool isMobile, string? oddsStyleId, string? sportId, string? leagueId, string? eventId, string? api, string language = "en", CancellationToken cancellationToken = default);

    /// <summary>
    /// (GP -> Tgs) Transfers from WHL asynchronous.
    /// </summary>
    /// <param name="apiUrl">The API URL.</param>
    /// <param name="agentUserName">Name of the agent user.</param>
    /// <param name="agentPassword">The agent password.</param>
    /// <param name="merchantCustomerCode">The merchant customer code.</param>
    /// <param name="refTransactionCode">The reference transaction code.</param>
    /// <param name="amount">The amount.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    ValueTask<TransferWHLResult> TransferFromWHLAsync(
        Uri apiUrl,
        string agentUserName,
        string agentPassword,
        string merchantCustomerCode,
        string refTransactionCode,
        decimal amount,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// (Tgs -> GP) Transfers to WHL asynchronous.
    /// </summary>
    /// <param name="apiUrl">The API URL.</param>
    /// <param name="agentUserName">Name of the agent user.</param>
    /// <param name="agentPassword">The agent password.</param>
    /// <param name="merchantCustomerCode">The merchant customer code.</param>
    /// <param name="refTransactionCode">The reference transaction code.</param>
    /// <param name="amount">The amount.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    ValueTask<TransferWHLResult> TransferToWHLAsync(
        Uri apiUrl,
        string agentUserName,
        string agentPassword,
        string merchantCustomerCode,
        string refTransactionCode,
        decimal amount,
        CancellationToken cancellationToken = default);
}
