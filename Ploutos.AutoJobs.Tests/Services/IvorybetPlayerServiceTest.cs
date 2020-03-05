using Fanex.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using Ploutos.AutoJobs.Models;
using Ploutos.AutoJobs.Services;
using Ploutos.AutoJobs.Services.Interfaces;
using Ploutos.Common.Services;
using Ploutos.Common.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Ploutos.AutoJobs.Tests.Services
{
    public class IvorybetPlayerServiceTest
    {
        private readonly IOptionsSnapshot<ApiConfigs> apiConfigs;
        private readonly IWebClientService webClientService;
        private readonly IDataService dataService;
        private readonly ILogger logger;
        private readonly IUtil util;
        private readonly ISupportApiService supportApiService;

        public IvorybetPlayerServiceTest()
        {
            apiConfigs = Substitute.For<IOptionsSnapshot<ApiConfigs>>();
            webClientService = Substitute.For<IWebClientService>();
            dataService = Substitute.For<IDataService>();
            logger = Substitute.For<ILogger>();
            util = Substitute.For<IUtil>();
            supportApiService = Substitute.For<ISupportApiService>();
        }

        [Fact]
        public async Task PushPlayers_WhenCalled_PushPlayers()
        {
            // Arrange
            var configs = new ApiConfigs
            {
                Ivorybet = new BrandApiConfigs
                {
                    PlayerConfigs = new UrlApiConfig
                    {
                        QueryBatchSize = 1,
                        SyncBatchSize = 1
                    }
                }
            };
            apiConfigs.Value.Returns(configs);
            var service = new IvorybetPlayerService(
                apiConfigs,
                webClientService,
                dataService,
                logger,
                util,
                supportApiService);

            dataService.GetPlayerProfiles(Arg.Any<string>(), Arg.Any<int>())
                .Returns(new List<PlayerProfile>());
            util.SplitToBatches(Arg.Any<List<PlayerProfile>>(), Arg.Any<int>())
                .Returns(new List<List<PlayerProfile>>());

            // Act
            await service.PushPlayers();

            // Assert
            await dataService.Received().GetPlayerProfiles(Arg.Any<string>(), Arg.Any<int>());
        }
    }    
}
