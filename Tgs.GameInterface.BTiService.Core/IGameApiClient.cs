using Tgs.GameInterface.BTiService.Core.Models;

namespace Tgs.GameInterface.BTiService.Core;

public interface IGameApiClient
{
    /// <summary>
    /// Checks the transaction asynchronous.
    /// </summary>
    /// <param name="apiUrl">The API URL.</param>
    /// <param name="agentUserName">Name of the agent user.</param>
    /// <param name="agentPassword">The agent password.</param>
    /// <param name="refTransactionCode">The reference transaction code.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    ValueTask<bool> CheckTransactionAsync(
        Uri apiUrl,
        string agentUserName,
        string agentPassword,
        string refTransactionCode,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates the new user asynchronous.
    /// </summary>
    /// <param name="apiUrl">The API URL.</param>
    /// <param name="agentUserName">Name of the agent user.</param>
    /// <param name="agentPassword">The agent password.</param>
    /// <param name="merchantCustomerCode">The merchant customer code.</param>
    /// <param name="currencyCode">The currency code.</param>
    /// <param name="firstName">The first name.</param>
    /// <param name="lastName">The last name.</param>
    /// <param name="countryCode">The country code.</param>
    /// <param name="city">The city.</param>
    /// <param name="customerDefaultLanguage">The customer default language.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    ValueTask CreateNewUserAsync(
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
       CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the customer authentication token asynchronous.
    /// </summary>
    /// <param name="apiUrl">The API URL.</param>
    /// <param name="agentUserName">Name of the agent user.</param>
    /// <param name="agentPassword">The agent password.</param>
    /// <param name="merchantCustomerCode">The merchant customer code.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    ValueTask<GetCustomerAuthTokenResult> GetCustomerAuthTokenAsync(
        Uri apiUrl,
        string agentUserName,
        string agentPassword,
        string merchantCustomerCode,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the balance asynchronous.
    /// </summary>
    /// <param name="apiUrl">The API URL.</param>
    /// <param name="agentUserName">Name of the agent user.</param>
    /// <param name="agentPassword">The agent password.</param>
    /// <param name="merchantCustomerCode">The merchant customer code.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    ValueTask<GetBalanceResult> GetBalanceAsync(
        Uri apiUrl,
        string agentUserName,
        string agentPassword,
        string merchantCustomerCode,
        CancellationToken cancellationToken = default);

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
