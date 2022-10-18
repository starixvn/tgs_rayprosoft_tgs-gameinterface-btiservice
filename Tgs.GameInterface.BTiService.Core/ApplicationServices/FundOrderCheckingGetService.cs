using Tgs.ServiceUtility;

namespace Tgs.GameInterface.BTiService.Core.ApplicationServices;

internal class FundOrderCheckingGetService : IGetService<FundOrderCheckingQuery, bool>
{
    private readonly IGameApiClient _client;

    public FundOrderCheckingGetService(IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async ValueTask<bool> GetAsync(FundOrderCheckingQuery command, CancellationToken cancellationToken = default)
        => await _client.CheckTransactionAsync(
            apiUrl: command.ApiUrl,
            agentUserName: command.AgentUserName,
            agentPassword: command.AgentPassword,
            refTransactionCode: command.TransactionId,
            cancellationToken: cancellationToken).ConfigureAwait(false);
}
