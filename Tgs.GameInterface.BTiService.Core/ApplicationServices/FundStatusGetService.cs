using Tgs.ServiceUtility;

namespace Tgs.GameInterface.BTiService.Core.ApplicationServices;

internal class FundStatusGetService : IGetService<FundStatusQuery, bool>
{
    private readonly IGameApiClient _client;

    public FundStatusGetService(IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public ValueTask<bool> GetAsync(FundStatusQuery command, CancellationToken cancellationToken = default)
        => _client.CheckTransactionAsync(
            apiUrl: command.ApiUrl,
            agentUserName: command.AgentUserName,
            agentPassword: command.AgentPassword,
            refTransactionCode: command.RefTransactionCode,
            cancellationToken: cancellationToken);
}
