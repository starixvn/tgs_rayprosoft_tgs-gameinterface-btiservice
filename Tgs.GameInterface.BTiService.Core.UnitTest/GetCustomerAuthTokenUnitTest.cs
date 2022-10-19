using System.Net;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RichardSzalay.MockHttp;
using Tgs.GameInterface.BTiService.Core.Exceptions;

namespace Tgs.GameInterface.BTiService.Core.UnitTest;

[TestClass]
public class GetCustomerAuthTokenUnitTest
{
    [TestMethod]
    public async Task 玩家不存在()
    {
        var apiUrl = new Uri("https://wallet.bti.bet/");
        var agentUserName = "PegaGaming_GPIRMB";
        var agentPassword = "IM60Ho7Y4B9C";
        var merchantCustomerCode = "4ucmqplxb029";

        var url = new Uri(apiUrl, "/api/walletservice/WHLCustomers/GetCustomerAuthToken");
        var response = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<MerchantResponse xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://networkpot.com/\">\r\n    <ErrorCode>CustomerNotFound</ErrorCode>\r\n    <CustomerID>-1</CustomerID>\r\n    <AuthToken />\r\n    <Balance>0</Balance>\r\n    <OpenBetsBalance>0</OpenBetsBalance>\r\n    <TransactionID>-1</TransactionID>\r\n</MerchantResponse>";
        var handler = new MockHttpMessageHandler();
        _ = handler.Expect(HttpMethod.Post, url.ToString())
                        .Respond(HttpStatusCode.OK, new StringContent(response));

        var fakeHttpClient = handler.ToHttpClient();
        var fakeLog = NullLogger<GameApiClient>.Instance;

        var sut = new GameApiClient(fakeHttpClient, fakeLog);

        var actual = await sut
            .GetCustomerAuthTokenAsync(
                apiUrl: apiUrl,
                agentUserName: agentUserName,
                agentPassword: agentPassword,
                merchantCustomerCode: merchantCustomerCode)
            .ConfigureAwait(false);

        Assert.IsNotNull(actual);
        Assert.IsFalse(actual.IsPlayerExist);
    }

    [TestMethod]
    public async Task 取得餘額()
    {
        var apiUrl = new Uri("https://wallet.bti.bet/");
        var agentUserName = "PegaGaming_GPIRMB";
        var agentPassword = "IM60Ho7Y4B9C";
        var merchantCustomerCode = "4ucmqplxb029";

        var url = new Uri(apiUrl, "/api/walletservice/WHLCustomers/GetCustomerAuthToken");
        var response = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<MerchantResponse xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://networkpot.com/\">\r\n    <ErrorCode>NoError</ErrorCode>\r\n    <CustomerID>-1</CustomerID>\r\n    <AuthToken>MGJlZTg1OTNkNDZhNjY1M2Q4NzA3MmM5Yzk2M2NlOGI6NzBhN2ZkZjM4YWE4NzIxY2FiNWNjZjM0MDUxYTBhNzQ4Y2YwZDM0Y2NkMWY4NzBiYzliNWU4MjAzNmNlOTE3YjQ0NDRlOTliMzRiMjZkNGJlNDUxODNhNGUzYTQ4NWQ1NDNiNDU4YmU0YWUxMGIyOGIxZWVjNDk1NjVmOGVmZGQ5Y2Y2ZmY3Y2Q4OGQ1OGJjNTFkZDY3ODViZDc0MmExMDJiYjhjYzQ1NTc1YjdjYzZkNmI4NTQ0MWI5YmMwZjM0</AuthToken>\r\n    <Balance>0</Balance>\r\n    <OpenBetsBalance>0</OpenBetsBalance>\r\n    <TransactionID>-1</TransactionID>\r\n</MerchantResponse>";
        var handler = new MockHttpMessageHandler();
        _ = handler.Expect(HttpMethod.Post, url.ToString())
                        .Respond(HttpStatusCode.OK, new StringContent(response));

        var fakeHttpClient = handler.ToHttpClient();
        var fakeLog = NullLogger<GameApiClient>.Instance;

        var sut = new GameApiClient(fakeHttpClient, fakeLog);

        var actual = await sut
            .GetCustomerAuthTokenAsync(
                apiUrl: apiUrl,
                agentUserName: agentUserName,
                agentPassword: agentPassword,
                merchantCustomerCode: merchantCustomerCode)
            .ConfigureAwait(false);

        Assert.IsNotNull(actual);
        Assert.IsTrue(actual.IsPlayerExist);
        Assert.IsFalse(string.IsNullOrWhiteSpace(actual.AuthToken));
    }

    [TestMethod]
    public async Task 其他錯誤()
    {
        var apiUrl = new Uri("https://wallet.bti.bet/");
        var agentUserName = "PegaGaming_GPIRMB";
        var agentPassword = "IM60Ho7Y4B9C";
        var merchantCustomerCode = "4ucmqplxb029";

        var url = new Uri(apiUrl, "/api/walletservice/WHLCustomers/GetCustomerAuthToken");
        var response = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<MerchantResponse xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://networkpot.com/\">\r\n    <ErrorCode>Exception</ErrorCode>\r\n    <CustomerID>-1</CustomerID>\r\n    <AuthToken />\r\n    <Balance>0</Balance>\r\n    <OpenBetsBalance>0</OpenBetsBalance>\r\n    <TransactionID>-1</TransactionID>\r\n</MerchantResponse>";
        var handler = new MockHttpMessageHandler();
        _ = handler.Expect(HttpMethod.Post, url.ToString())
                        .Respond(HttpStatusCode.OK, new StringContent(response));

        var fakeHttpClient = handler.ToHttpClient();
        var fakeLog = NullLogger<GameApiClient>.Instance;

        var sut = new GameApiClient(fakeHttpClient, fakeLog);

        _ = await Assert
            .ThrowsExceptionAsync<GameProviderResponseErrorException>(() =>
                sut.GetCustomerAuthTokenAsync(
                    apiUrl: apiUrl,
                    agentUserName: agentUserName,
                    agentPassword: agentPassword,
                    merchantCustomerCode: merchantCustomerCode)
                .AsTask())
            .ConfigureAwait(false);
    }
}
