using Fanex.Logging;
using FluentAssertions;
using Newtonsoft.Json;
using NSubstitute;
using Ploutos.AutoJobs.Models;
using Ploutos.AutoJobs.Services;
using Ploutos.AutoJobs.Services.Interfaces;
using Ploutos.Common.Services;
using Ploutos.Common.Utils;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Ploutos.AutoJobs.Tests.Services
{
    public class SyncPlayerBaseTest
    {
        private readonly IWebClientService webClientService;
        private readonly IDataService dataService;
        private readonly ILogger logger;
        private readonly IUtil util;
        private readonly ISupportApiService supportApiService;

        public SyncPlayerBaseTest()
        {
            webClientService = Substitute.For<IWebClientService>();
            dataService = Substitute.For<IDataService>();
            logger = Substitute.For<ILogger>();
            util = Substitute.For<IUtil>();
            supportApiService = Substitute.For<ISupportApiService>();
        }

        [Fact]
        public async Task PushPlayers_WhenStatusCodeOK_UpdateProfileSuccess()
        {
            // Arrange
            var apiConfigs = new BrandApiConfigs
            {
                PlayerConfigs = new UrlApiConfig { QueryBatchSize = 1, SyncBatchSize = 1 }
            };
            var syncPlayerBase = new SyncPlayerBase(apiConfigs, webClientService, dataService, logger, util, supportApiService);

            var playerProfiles = new List<PlayerProfile> {
                new PlayerProfile { }
            };
            dataService.GetPlayerProfiles(Arg.Any<string>(), Arg.Any<int>()).Returns(playerProfiles);
            util.SplitToBatches(Arg.Any<List<PlayerProfile>>(), Arg.Any<int>())
                .Returns(new List<List<PlayerProfile>> { playerProfiles });

            var restResponse = new RestResponse()
            {
                StatusCode = HttpStatusCode.OK,
                Content = "ac"
            };
            webClientService.PostAsync<PlayerProfile>(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<PlayerProfile>())
                .Returns(restResponse);

            var profileResponse = new ProfileResponse
            {
                Status = Constant.Constants.StatusSuccess,
                Data = new ProfileResponseData { PlayerAccountNumber = "xx" }
            };
            util.ConvertJsonResponse<ProfileResponse>(Arg.Any<string>()).Returns(profileResponse);
            supportApiService.UpdateResponseFromSupportAPI(Arg.Any<ProfileResponse>(), Arg.Any<string>(), Arg.Any<SupportApiConfigs>())
                .Returns(profileResponse);

            List<UpdatePlayerProfile> updateProfiles = null;
            await dataService.UpdatePlayerProfiles(Arg.Any<string>(), Arg.Do<List<UpdatePlayerProfile>>(x => updateProfiles = x));

            // Act
            await syncPlayerBase.PushPlayers();

            // Assert
            updateProfiles[0].IsError.Should().Be(!profileResponse.IsSuccess);
            updateProfiles[0].PlayerCode.Should().Be(profileResponse.Data.PlayerAccountNumber);
            updateProfiles[0].ResponseResult.Should().Be(JsonConvert.SerializeObject(profileResponse));
        }

        [Fact]
        public async Task PushPlayers_WhenStatusCodeBadRequest_UpdateProfileFailed()
        {
            // Arrange
            var apiConfigs = new BrandApiConfigs
            {
                PlayerConfigs = new UrlApiConfig { QueryBatchSize = 1, SyncBatchSize = 1 }
            };
            var syncPlayerBase = new SyncPlayerBase(apiConfigs, webClientService, dataService, logger, util, supportApiService);

            var playerProfiles = new List<PlayerProfile> {
                new PlayerProfile { }
            };
            dataService.GetPlayerProfiles(Arg.Any<string>(), Arg.Any<int>()).Returns(playerProfiles);
            util.SplitToBatches(Arg.Any<List<PlayerProfile>>(), Arg.Any<int>())
                .Returns(new List<List<PlayerProfile>> { playerProfiles });

            var restResponse = new RestResponse()
            {
                StatusCode = HttpStatusCode.BadRequest,
                Content = "ac"
            };
            webClientService.PostAsync<PlayerProfile>(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<PlayerProfile>())
                .Returns(restResponse);

            var profileResponse = new ProfileResponse
            {
                Status = Constant.Constants.StatusSuccess,
                Data = new ProfileResponseData { PlayerAccountNumber = "xx" }
            };
            util.ConvertJsonResponse<ProfileResponse>(Arg.Any<string>()).Returns(profileResponse);
            supportApiService.UpdateResponseFromSupportAPI(Arg.Any<ProfileResponse>(), Arg.Any<string>(), Arg.Any<SupportApiConfigs>())
                .Returns(profileResponse);

            List<UpdatePlayerProfile> updateProfiles = null;
            await dataService.UpdatePlayerProfiles(Arg.Any<string>(), Arg.Do<List<UpdatePlayerProfile>>(x => updateProfiles = x));

            // Act
            await syncPlayerBase.PushPlayers();

            // Assert
            updateProfiles[0].IsError.Should().Be(true);
            updateProfiles[0].ResponseResult.Should().Be(restResponse.Content);
        }

        [Fact]
        public async Task PushPlayers_WhenStatusCodeInternalServerError_UpdateProfileSuccess()
        {
            // Arrange
            var apiConfigs = new BrandApiConfigs
            {
                PlayerConfigs = new UrlApiConfig { QueryBatchSize = 1, SyncBatchSize = 1 }
            };
            var syncPlayerBase = new SyncPlayerBase(apiConfigs, webClientService, dataService, logger, util, supportApiService);

            var playerProfiles = new List<PlayerProfile> {
                new PlayerProfile { }
            };
            dataService.GetPlayerProfiles(Arg.Any<string>(), Arg.Any<int>()).Returns(playerProfiles);
            util.SplitToBatches(Arg.Any<List<PlayerProfile>>(), Arg.Any<int>())
                .Returns(new List<List<PlayerProfile>> { playerProfiles });

            var restResponse = new RestResponse()
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Content = "ac"
            };
            webClientService.PostAsync<PlayerProfile>(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<PlayerProfile>())
                .Returns(restResponse);

            var profileResponse = new ProfileResponse
            {
                Status = Constant.Constants.StatusSuccess,
                Data = new ProfileResponseData { PlayerAccountNumber = "xx" }
            };
            util.ConvertJsonResponse<ProfileResponse>(Arg.Any<string>()).Returns(profileResponse);
            supportApiService.UpdateResponse(Arg.Any<ProfileResponse>(), Arg.Any<string>(), Arg.Any<SupportApiConfigs>())
                .Returns(profileResponse);

            List<UpdatePlayerProfile> updateProfiles = null;
            await dataService.UpdatePlayerProfiles(Arg.Any<string>(), Arg.Do<List<UpdatePlayerProfile>>(x => updateProfiles = x));

            // Act
            await syncPlayerBase.PushPlayers();

            // Assert
            updateProfiles[0].IsError.Should().Be(!profileResponse.IsSuccess);
            updateProfiles[0].PlayerCode.Should().Be(profileResponse.Data.PlayerAccountNumber);
            updateProfiles[0].ResponseResult.Should().Be(JsonConvert.SerializeObject(profileResponse));
        }

        [Fact]
        public async Task PushPlayers_WhenStatusCodeOthers_UpdateProfileFailedAndWriteLog()
        {
            // Arrange
            var apiConfigs = new BrandApiConfigs
            {
                PlayerConfigs = new UrlApiConfig { QueryBatchSize = 1, SyncBatchSize = 1 }
            };
            var syncPlayerBase = new SyncPlayerBase(apiConfigs, webClientService, dataService, logger, util, supportApiService);

            var playerProfiles = new List<PlayerProfile> {
                new PlayerProfile { }
            };
            dataService.GetPlayerProfiles(Arg.Any<string>(), Arg.Any<int>()).Returns(playerProfiles);
            util.SplitToBatches(Arg.Any<List<PlayerProfile>>(), Arg.Any<int>())
                .Returns(new List<List<PlayerProfile>> { playerProfiles });

            var restResponse = new RestResponse()
            {
                StatusCode = HttpStatusCode.BadGateway,
                Content = "errorss"
            };
            webClientService.PostAsync<PlayerProfile>(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<PlayerProfile>())
                .Returns(restResponse);

            var profileResponse = new ProfileResponse
            {
                Status = Constant.Constants.StatusSuccess,
                Data = new ProfileResponseData { PlayerAccountNumber = "xx" }
            };
            util.ConvertJsonResponse<ProfileResponse>(Arg.Any<string>()).Returns(profileResponse);
            supportApiService.UpdateResponseFromSupportAPI(Arg.Any<ProfileResponse>(), Arg.Any<string>(), Arg.Any<SupportApiConfigs>())
                .Returns(profileResponse);

            List<UpdatePlayerProfile> updateProfiles = null;
            await dataService.UpdatePlayerProfiles(Arg.Any<string>(), Arg.Do<List<UpdatePlayerProfile>>(x => updateProfiles = x));

            var logMsg = "log msg";
            util.BuildLogMessage(Arg.Any<Exception>()).Returns(logMsg);

            // Act
            await syncPlayerBase.PushPlayers();

            // Assert
            updateProfiles[0].IsError.Should().Be(true);
            updateProfiles[0].ResponseResult.Should().Be((new HttpRequestException(restResponse.Content)).Message);
            await logger.Received().ErrorAsync(logMsg);
        }
    }
}
