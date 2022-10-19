using System.Net;
using System.Text.Json;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.BTiService.Core.UnitTest;

[TestClass]
public class GetTokenUnitTest
{
    [TestMethod]
    public async Task Normal_Process()
    {
        var apiUrl = new Uri("https://asia-east2-bq-data-api.cloudfunctions.net/");
        var agentUserName = "PegaGaming_GPIRMB";
        var agentPassword = "IM60Ho7Y4B9C";
        var response = new
        {
            errorCode = 0,
            token = Guid.NewGuid().ToString("N")
        };
        var url = new Uri(apiUrl, $"/authorize_v2");
        var handler = new MockHttpMessageHandler();
        _ = handler.Expect(HttpMethod.Post, url.ToString())
                        .Respond(HttpStatusCode.OK, new StringContent(JsonSerializer.Serialize(response)));

        var fakeHttpClient = handler.ToHttpClient();
        var fakeLog = NullLogger<GameDataApiClient>.Instance;

        var sut = new GameDataApiClient(fakeHttpClient, fakeLog);

        var actual = await sut
            .GetTokenAsync(
                apiUrl: apiUrl,
                agentUserName: agentUserName,
                agentPassword: agentPassword)
            .ConfigureAwait(false);

        Assert.IsNotNull(actual);
        Assert.IsTrue(actual.Equals(response.token));
    }
}
