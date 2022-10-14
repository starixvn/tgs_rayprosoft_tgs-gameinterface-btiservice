using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.BTiService.Repositories.UnitTest;

[TestClass]
public class GetTokenUnitTest
{
    [TestMethod]
    public async Task Normal_Process()
    {
        var handler = new MockHttpMessageHandler();

        var fakeHttpClient = handler.ToHttpClient();
        var fakeLog = NullLogger<GameApiClient>.Instance;

        var sut = new GameApiClient(fakeHttpClient, fakeLog);

        var apiUrl = new Uri("http://localhost/");
        var agentUserName = "testAgentUserName";
        var agentPassword = "testAgentPassword";

        var actual = await sut
            .GetTokenAsync(
                apiUrl: apiUrl,
                agentUserName: agentUserName,
                agentPassword: agentPassword)
            .ConfigureAwait(false);

        Assert.IsNotNull(actual);
    }
}
