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
        private readonly IOptionsSnapshot<ProxySettings> proxySettings;

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
            var settings = new ProxySettings
            {
                IsUseProxy = true,
                ProxyAddress = "1.1.1.1:8801",
                BypassOnLocal = true
            };

            proxySettings.Value.Returns(settings);
            var webClientService = new WebClientService(proxySettings);

            string domain = "http://10.18.200.114:7555";
            string url = null;
            string token = null;
            var parameters = new List<Parameter> { new Parameter { Name = "username", Value = "" } };

            // Act
            var result = await webClientService.GetAsync(domain, url, token, parameters);

            // Assert
            Assert.Equal(result.StatusCode, dataResponse.StatusCode);
        }

        [Fact]
        public async Task PostJsonAsync_ParamAreInvalid_ReturnResultExpect()
        {
            // Arrange
            var dataResponse = new RestResponse
            {
                StatusCode = 0
            };
            var settings = new ProxySettings
            {
                IsUseProxy = true,
                ProxyAddress = "1.1.1.1:8801",
                BypassOnLocal = true
            };

            proxySettings.Value.Returns(settings);
            var webClientService = new WebClientService(proxySettings);

            string domain = "http://10.18.200.114:7555";
            string url = null;
            string token = null;
            var parameters = new List<Parameter> { new Parameter { Name = "username", Value = "" } };

            // Act
            var result = await webClientService.PostJsonAsync(domain, url, token, parameters);

            // Assert
            Assert.Equal(result.StatusCode, dataResponse.StatusCode);
        }

        [Fact]
        public async Task PostAsync_ParamAreInvalid_ReturnResultExpect()
        {
            // Arrange
            var dataResponse = new RestResponse
            {
                StatusCode = 0
            };
            var settings = new ProxySettings
            {
                IsUseProxy = true,
                ProxyAddress = "1.1.1.1:8801",
                BypassOnLocal = true
            };

            proxySettings.Value.Returns(settings);
            var webClientService = new WebClientService(proxySettings);

            string domain = "http://10.18.200.114:7555";
            string url = null;
            string token = "token abc";
            var parameters = new List<Parameter> { new Parameter { Name = "username", Value = "" } };

            // Act
            var result = await webClientService.PostAsync(domain, url, token, parameters);

            // Assert
            Assert.Equal(result.StatusCode, dataResponse.StatusCode);
        }
    }
}
