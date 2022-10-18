using System.Net;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RichardSzalay.MockHttp;
using Tgs.GameInterface.BTiService.Core.Exceptions;

namespace Tgs.GameInterface.BTiService.Core.UnitTest;

[TestClass]
public class CreateNewUserUnitTest
{
    [TestMethod]
    public async Task 玩家已經存在()
    {
        var apiUrl = new Uri("https://wallet.bti.bet/");
        var agentUserName = "PegaGaming_GPIRMB";
        var agentPassword = "IM60Ho7Y4B9C";
        var merchantCustomerCode = "4ucmqplxb029";

        var url = new Uri(apiUrl, "/api/walletservice/WHLCustomers/GetBalance");
        var response = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<MerchantResponse xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://networkpot.com/\">\r\n    <ErrorCode>CustomerNotFound</ErrorCode>\r\n    <CustomerID>-1</CustomerID>\r\n    <AuthToken />\r\n    <Balance>0</Balance>\r\n    <OpenBetsBalance>0</OpenBetsBalance>\r\n    <TransactionID>-1</TransactionID>\r\n</MerchantResponse>";
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
                merchantCustomerCode: merchantCustomerCode)
            .ConfigureAwait(false);

        Assert.IsNotNull(actual);
        Assert.IsFalse(actual.IsPlayerExist);
    }

    [TestMethod]
    public async Task 創建玩家成功()
    {
        var apiUrl = new Uri("https://wallet.bti.bet/");
        var agentUserName = "PegaGaming_GPIRMB";
        var agentPassword = "IM60Ho7Y4B9C";
        var merchantCustomerCode = "4ucmqplxb029";
        var currencyCode = "CNY";
        var firstName = "GPI";
        var lastName = "4ucmqplxb029";
        var countryCode = "CN";
        var city = "Beijing";
        var customerDefaultLanguage = "zh";
        var url = new Uri(apiUrl, "/api/walletservice/WHLCustomers/CreateUserNew");
        var response = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<MerchantResponse xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://networkpot.com/\">\r\n    <ErrorCode>DuplicateMerchantCustomerCode</ErrorCode>\r\n    <CustomerID>-1</CustomerID>\r\n    <AuthToken />\r\n    <Balance>0</Balance>\r\n    <OpenBetsBalance>0</OpenBetsBalance>\r\n    <TransactionID>-1</TransactionID>\r\n</MerchantResponse>";
        var handler = new MockHttpMessageHandler();
        _ = handler.Expect(HttpMethod.Post, url.ToString())
                        .Respond(HttpStatusCode.OK, new StringContent(response));

        var fakeHttpClient = handler.ToHttpClient();
        var fakeLog = NullLogger<GameApiClient>.Instance;

        var sut = new GameApiClient(fakeHttpClient, fakeLog);

        await sut
            .CreateNewUserAsync(
                apiUrl: apiUrl,
                agentUserName: agentUserName,
                agentPassword: agentPassword,
                merchantCustomerCode: merchantCustomerCode,
                currencyCode: currencyCode,
                firstName: firstName,
                lastName: lastName,
                countryCode: countryCode,
                city: city,
                customerDefaultLanguage: customerDefaultLanguage)
            .ConfigureAwait(false);
    }

    [TestMethod]
    public async Task 其他錯誤()
    {
        var apiUrl = new Uri("https://wallet.bti.bet/");
        var agentUserName = "PegaGaming_GPIRMB";
        var agentPassword = "IM60Ho7Y4B9C";
        var merchantCustomerCode = "4ucmqplxb029";
        var currencyCode = "CNY";
        var firstName = "GPI";
        var lastName = "4ucmqplxb029";
        var countryCode = "CN";
        var city = "Beijing";
        var customerDefaultLanguage = "zh";
        var url = new Uri(apiUrl, "/api/walletservice/WHLCustomers/CreateUserNew");
        var response = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<MerchantResponse xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://networkpot.com/\">\r\n    <ErrorCode>Exception</ErrorCode>\r\n    <CustomerID>-1</CustomerID>\r\n    <AuthToken />\r\n    <Balance>0</Balance>\r\n    <OpenBetsBalance>0</OpenBetsBalance>\r\n    <TransactionID>-1</TransactionID>\r\n</MerchantResponse>";
        var handler = new MockHttpMessageHandler();
        _ = handler.Expect(HttpMethod.Post, url.ToString())
                        .Respond(HttpStatusCode.OK, new StringContent(response));

        var fakeHttpClient = handler.ToHttpClient();
        var fakeLog = NullLogger<GameApiClient>.Instance;

        var sut = new GameApiClient(fakeHttpClient, fakeLog);

        _ = await Assert
            .ThrowsExceptionAsync<GameProviderResponseErrorException>(() =>
                sut.CreateNewUserAsync(
                    apiUrl: apiUrl,
                    agentUserName: agentUserName,
                    agentPassword: agentPassword,
                    merchantCustomerCode: merchantCustomerCode,
                    currencyCode: currencyCode,
                    firstName: firstName,
                    lastName: lastName,
                    countryCode: countryCode,
                    city: city,
                    customerDefaultLanguage: customerDefaultLanguage)
                .AsTask())
            .ConfigureAwait(false);
    }
}
