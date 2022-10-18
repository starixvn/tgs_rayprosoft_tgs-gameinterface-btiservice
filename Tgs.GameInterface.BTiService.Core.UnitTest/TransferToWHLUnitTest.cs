using System.Net;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RichardSzalay.MockHttp;
using Tgs.GameInterface.BTiService.Core.Exceptions;

namespace Tgs.GameInterface.BTiService.Core.UnitTest;

[TestClass]
public class TransferToWHLUnitTest
{
    [TestMethod]
    public async Task 主錢包轉入遊戲廠商成功()
    {
        var apiUrl = new Uri("https://wallet.bti.bet/");
        var agentUserName = "PegaGaming";
        var agentPassword = "lQH8635mmocK";
        var merchantCustomerCode = "test";
        var refTransactionCode = Guid.NewGuid().ToString("N");
        var amount = 10m;
        var transactionId = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        var balance = 100m + amount;

        var url = new Uri(apiUrl, "/api/walletservice/WHLCustomers/TransferToWHL");
        var response = $"<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<MerchantResponse xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://networkpot.com/\">\r\n    <ErrorCode>NoError</ErrorCode>\r\n    <CustomerID>-1</CustomerID>\r\n    <AuthToken />\r\n    <Balance>{balance}</Balance>\r\n    <OpenBetsBalance></OpenBetsBalance>\r\n    <TransactionID>{transactionId}</TransactionID>\r\n</MerchantResponse>";
        var handler = new MockHttpMessageHandler();
        _ = handler.Expect(HttpMethod.Post, url.ToString())
                        .Respond(HttpStatusCode.OK, new StringContent(response));

        var fakeHttpClient = handler.ToHttpClient();
        var fakeLog = NullLogger<GameApiClient>.Instance;

        var sut = new GameApiClient(fakeHttpClient, fakeLog);

        var actual = await sut
            .TransferToWHLAsync(
                apiUrl: apiUrl,
                agentUserName: agentUserName,
                agentPassword: agentPassword,
                merchantCustomerCode: merchantCustomerCode,
                refTransactionCode: refTransactionCode,
                amount: amount)
            .ConfigureAwait(false);

        Assert.IsNotNull(actual);
        Assert.AreEqual(refTransactionCode, actual.RefTransactionCode);
        Assert.AreEqual(transactionId.ToString(), actual.TransactionId);
        Assert.AreEqual(balance, actual.Balance);
    }

    [TestMethod]
    public async Task 其他錯誤()
    {
        var apiUrl = new Uri("https://wallet.bti.bet/");
        var agentUserName = "PegaGaming";
        var agentPassword = "lQH8635mmocK";
        var merchantCustomerCode = "test";
        var refTransactionCode = Guid.NewGuid().ToString("N");
        var amount = 10m;

        var url = new Uri(apiUrl, "/api/walletservice/WHLCustomers/TransferToWHL");
        var response = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<MerchantResponse xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://networkpot.com/\">\r\n    <ErrorCode>Exception</ErrorCode>\r\n    <CustomerID>-1</CustomerID>\r\n    <AuthToken />\r\n    <Balance>0</Balance>\r\n    <OpenBetsBalance>0</OpenBetsBalance>\r\n    <TransactionID>-1</TransactionID>\r\n</MerchantResponse>";
        var handler = new MockHttpMessageHandler();
        _ = handler.Expect(HttpMethod.Post, url.ToString())
                        .Respond(HttpStatusCode.OK, new StringContent(response));

        var fakeHttpClient = handler.ToHttpClient();
        var fakeLog = NullLogger<GameApiClient>.Instance;

        var sut = new GameApiClient(fakeHttpClient, fakeLog);

        _ = await Assert
            .ThrowsExceptionAsync<GameProviderResponseErrorException>(() =>
                sut.TransferToWHLAsync(
                    apiUrl: apiUrl,
                    agentUserName: agentUserName,
                    agentPassword: agentPassword,
                    merchantCustomerCode: merchantCustomerCode,
                    refTransactionCode: refTransactionCode,
                    amount: amount)
                .AsTask())
            .ConfigureAwait(false);
    }
}
