using Fanex.Logging;
using FluentAssertions;
using NSubstitute;
using Ploutos.AutoJobs.Models;
using Ploutos.AutoJobs.Services;
using Ploutos.Common.Services;
using Ploutos.Common.Utils;
using RestSharp;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Ploutos.AutoJobs.Tests.Services
{
    public class SupportApiServiceTest
    {
        private readonly IWebClientService webClientService;
        private readonly ILogger logger;
        private readonly IUtil util;

        public SupportApiServiceTest()
        {
            webClientService = Substitute.For<IWebClientService>();
            logger = Substitute.For<ILogger>();
            util = Substitute.For<IUtil>();
        }

        [Fact]
        public async Task UpdateResponseFromSupportAPI_WhenProfileResponseSuccess_DoNothing()
        {
            // Arrange
            var service = new SupportApiService(webClientService, logger, util);
            var profileResponse = new ProfileResponse
            {
                Status = Constant.Constants.StatusSuccess
            };

            // Act
            var result = await service.UpdateResponseFromSupportAPI(profileResponse, string.Empty, new SupportApiConfigs());

            // Assert
            result.Status.Should().Be(profileResponse.Status);
        }

        [Fact]
        public async Task UpdateResponseFromSupportAPI_WhenNotUserExist_DoNothing()
        {
            // Arrange
            var service = new SupportApiService(webClientService, logger, util);
            var profileResponse = new ProfileResponse
            {
                Status = "acb",
                Messages = new ResponseMessage
                {
                    Errors = new List<ResponseError>
                    {
                        new ResponseError { Code = "x", Description = "y"}
                    }
                }
            };

            // Act
            var result = await service.UpdateResponseFromSupportAPI(profileResponse, string.Empty, new SupportApiConfigs());

            // Assert
            result.Status.Should().Be(profileResponse.Status);
        }

        [Fact]
        public async Task UpdateResponseFromSupportAPI_WhenSuccess_UpdateProfileResponse()
        {
            // Arrange
            var service = new SupportApiService(webClientService, logger, util);
            var profileResponse = new ProfileResponse
            {
                Status = "acb",// failed
                Messages = new ResponseMessage
                {
                    Errors = new List<ResponseError>
                    {
                        new ResponseError { Code = "USER-ERROR-10002", Description = "Username already in use."}
                    }
                }
            };

            var authResponse = new RestResponse
            {
                StatusCode = HttpStatusCode.OK,
                Content = "token abc"
            };
            webClientService.PostAsync<SupportApiAuthObj>(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<SupportApiAuthObj>())
                .Returns(authResponse);

            var dataResponse = new RestResponse
            {
                StatusCode = HttpStatusCode.OK,
                Content = "player code"
            };
            webClientService.GetAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<List<Parameter>>())
                .Returns(dataResponse);

            // Act
            var result = await service.UpdateResponseFromSupportAPI(profileResponse, string.Empty, new SupportApiConfigs());

            // Assert
            result.Status.Should().Be(Constant.Constants.StatusSuccess);
            result.Data.PlayerAccountNumber.Should().Be(dataResponse.Content);
        }

        [Fact]
        public async Task UpdateResponseFromSupportAPI_WhenUnauthorized_WriteLog()
        {
            // Arrange
            var service = new SupportApiService(webClientService, logger, util);
            var profileResponse = new ProfileResponse
            {
                Status = "acb",// failed
                Messages = new ResponseMessage
                {
                    Errors = new List<ResponseError>
                    {
                        new ResponseError { Code = "USER-ERROR-10002", Description = "Username already in use."}
                    }
                }
            };

            var authResponse = new RestResponse
            {
                StatusCode = HttpStatusCode.OK,
                Content = "token abc"
            };
            webClientService.PostAsync<SupportApiAuthObj>(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<SupportApiAuthObj>())
                .Returns(authResponse);

            var dataResponse = new RestResponse
            {
                StatusCode = HttpStatusCode.Unauthorized
            };
            webClientService.GetAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<List<Parameter>>())
                .Returns(dataResponse);

            // Act
            var result = await service.UpdateResponseFromSupportAPI(profileResponse, string.Empty, new SupportApiConfigs());

            // Assert
            await logger.Received().ErrorAsync(Arg.Any<string>());
            result.Status.Should().Be(profileResponse.Status);
        }

        [Fact]
        public async Task UpdateResponseFromSupportAPI_WhenOtherErrors_WriteLog()
        {
            // Arrange
            var service = new SupportApiService(webClientService, logger, util);
            var profileResponse = new ProfileResponse
            {
                Status = "acb",// failed
                Messages = new ResponseMessage
                {
                    Errors = new List<ResponseError>
                    {
                        new ResponseError { Code = "USER-ERROR-10002", Description = "Username already in use."}
                    }
                }
            };

            var authResponse = new RestResponse
            {
                StatusCode = HttpStatusCode.OK,
                Content = "token abc"
            };
            webClientService.PostAsync<SupportApiAuthObj>(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<SupportApiAuthObj>())
                .Returns(authResponse);

            var dataResponse = new RestResponse
            {
                StatusCode = HttpStatusCode.BadGateway
            };
            webClientService.GetAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<List<Parameter>>())
                .Returns(dataResponse);

            // Act
            var result = await service.UpdateResponseFromSupportAPI(profileResponse, string.Empty, new SupportApiConfigs());

            // Assert
            await logger.Received().ErrorAsync(Arg.Any<string>());
            result.Status.Should().Be(profileResponse.Status);
        }
    }
}
