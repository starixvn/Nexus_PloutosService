namespace Ploutos.Common.Tests.Services
{
    using Microsoft.Extensions.Options;
    using NSubstitute;
    using Ploutos.Common.Models;
    using Ploutos.Common.Services;
    using RestSharp;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Xunit;

    public class WebClientServiceTests
    {
        private IOptionsSnapshot<ProxySettings> proxySettings;
        private IWebClientService webClientService;
        private IRestClient webClient;

        public WebClientServiceTests()
        {
            proxySettings = Substitute.For<IOptionsSnapshot<ProxySettings>>();
        }

        [Fact]
        public async Task GetAsync_ParamAreInvalid_ReturnResultExpect()
        {
            // Arrange
            var dataResponse = new RestResponse
            {
                StatusCode = 0
            };

            string domain = "http://localhost";
            string url = null;
            string token = "token abc";
            var parameters = new List<Parameter> { new Parameter { Name = "username", Value = "" } };

            webClientService = GetWebClientService();
            webClient.ExecuteGetTaskAsync(Arg.Any<RestRequest>()).Returns(dataResponse);
            
            // Act
            var result = await webClientService.GetAsync(domain, url, token, parameters);

            // Assert
            Assert.Equal(result.StatusCode, dataResponse.StatusCode);
        }

        [Fact]
        public async Task PostAsync_ParamAreInvalid_ReturnExpectedResponseStatus()
        {
            // Arrange
            var dataResponse = new RestResponse
            {
                ResponseStatus = ResponseStatus.Error
            };

            string domain = "http://localhost";
            string url = null;
            string token = "token abc";
            var parameters = new { test = "test" };

            webClientService = GetWebClientService();
            webClient.ExecutePostTaskAsync(Arg.Any<RestRequest>()).Returns(dataResponse);

            // Act
            var result = await webClientService.PostAsync(domain, url, token, parameters);

            // Assert
            Assert.Equal(result.ResponseStatus, dataResponse.ResponseStatus);
        }

        [Fact]
        public async Task PostFormAsync_ParamAreInvalid_ReturnExpectedResponseStatus()
        {
            // Arrange
            var dataResponse = new RestResponse
            {
                ResponseStatus =  ResponseStatus.Completed
            };

            string domain = "http://localhost";
            string url = null;
            string token = "token abc";
            var parameters = new { test = "test" };

            webClientService = GetWebClientService();
            webClient.ExecutePostTaskAsync(Arg.Any<RestRequest>()).Returns(dataResponse);

            // Act
            var result = await webClientService.PostFormAsync(domain, url, token, parameters);

            // Assert
            Assert.Equal(result.ResponseStatus, dataResponse.ResponseStatus);
        }

        [Fact]
        public async Task PostJsonAsync_ParamAreInvalid_ReturnExpectedStatusCode()
        {
            // Arrange
            var dataResponse = new RestResponse
            {
                StatusCode = 0
            };
            webClientService = GetWebClientService();

            string domain = "http://localhost";
            string url = null;
            string token = "token abc";
            var parameters = new { test = "test" };

            // Act
            var result = await webClientService.PostJsonAsync(domain, url, token, parameters);

            // Assert
            Assert.Equal(result.StatusCode, dataResponse.StatusCode);
        }

        private WebClientService GetWebClientService()
        {
            proxySettings = Substitute.For<IOptionsSnapshot<ProxySettings>>();

            var settings = new ProxySettings
            {
                IsUseProxy = true,
                ProxyAddress = "1.1.1.1:8801",
                BypassOnLocal = true
            };

            webClient = Substitute.For<IRestClient>();
            proxySettings.Value.Returns(settings);

            return new WebClientService(webClient, proxySettings);
        }
    }

    public class AuthParam
    {
        public string response_type { set; get; }
        public string client_id { set; get; }
        public string client_secret { set; get; }
    }
}
