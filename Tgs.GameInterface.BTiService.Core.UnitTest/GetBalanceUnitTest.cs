using System.Net;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.BTiService.Core.UnitTest;

[TestClass]
public class GetBalanceUnitTest
{
    [TestMethod]
    public async Task 玩家不存在()
    {
        var apiUrl = new Uri("https://wallet.bti.bet/api/walletservice/WHLCustomers/GetBalance/");
        var agentUserName = "PegaGaming";
        var agentPassword = "lQH8635mmocK";
        var playerCode = "test";
        var response = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<MerchantResponse xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://networkpot.com/\">\r\n    <ErrorCode>CustomerNotFound</ErrorCode>\r\n    <CustomerID>-1</CustomerID>\r\n    <AuthToken />\r\n    <Balance>0</Balance>\r\n    <OpenBetsBalance>0</OpenBetsBalance>\r\n    <TransactionID>-1</TransactionID>\r\n</MerchantResponse>";
        var url = new Uri(apiUrl, $"/api/walletservice/WHLCustomers/GetBalance");
        var handler = new MockHttpMessageHandler();
        _ = handler.Expect(HttpMethod.Post, url.ToString())
                        .Respond(HttpStatusCode.OK, new StringContent(response));

        var fakeHttpClient = handler.ToHttpClient();
        var fakeLog = NullLogger<GameApiClient>.Instance;

        var sut = new GameApiClient(fakeHttpClient, fakeLog);

        var actual = await sut
            .GetBalanceAsync(
                apiUrl: apiUrl,
                agentUserName: agentUserName,
                agentPassword: agentPassword,
                playerCode: playerCode)
            .ConfigureAwait(false);

        Assert.IsNotNull(actual);
        Assert.IsFalse(actual.IsPlayerExist);
    }

    [TestMethod]
    public async Task 取得餘額()
    {
        var apiUrl = new Uri("https://wallet.bti.bet/api/walletservice/WHLCustomers/GetBalance/");
        var agentUserName = "PegaGaming";
        var agentPassword = "lQH8635mmocK";
        var playerCode = "test";
        var response = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<MerchantResponse xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://networkpot.com/\">\r\n    <ErrorCode>NoError</ErrorCode>\r\n    <CustomerID>1</CustomerID>\r\n    <AuthToken />\r\n    <Balance>100</Balance>\r\n    <OpenBetsBalance>0</OpenBetsBalance>\r\n    <TransactionID>-1</TransactionID>\r\n</MerchantResponse>";
        var url = new Uri(apiUrl, $"/api/walletservice/WHLCustomers/GetBalance");
        var handler = new MockHttpMessageHandler();
        _ = handler.Expect(HttpMethod.Post, url.ToString())
                        .Respond(HttpStatusCode.OK, new StringContent(response));

        var fakeHttpClient = handler.ToHttpClient();
        var fakeLog = NullLogger<GameApiClient>.Instance;

        var sut = new GameApiClient(fakeHttpClient, fakeLog);

        var actual = await sut
            .GetBalanceAsync(
                apiUrl: apiUrl,
                agentUserName: agentUserName,
                agentPassword: agentPassword,
                playerCode: playerCode)
            .ConfigureAwait(false);

        Assert.IsNotNull(actual);
        Assert.IsTrue(actual.IsPlayerExist);
        Assert.AreEqual(100m, actual.Balance);
    }
}
