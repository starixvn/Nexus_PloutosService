using Fanex.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using Ploutos.AutoJobs.Models;
using Ploutos.AutoJobs.Services;
using Ploutos.AutoJobs.Services.Interfaces;
using Ploutos.Common.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Ploutos.AutoJobs.Tests.Services
{
    public class Nova88TicketServiceTest
    {
        private readonly IOptionsSnapshot<ApiConfigs> apiConfigs;
        private readonly IWebClientService webClientService;
        private readonly IDataService dataService;
        private readonly ILogger logger;
        private readonly IUtil util;

        public Nova88TicketServiceTest()
        {
            apiConfigs = Substitute.For<IOptionsSnapshot<ApiConfigs>>();
            webClientService = Substitute.For<IWebClientService>();
            dataService = Substitute.For<IDataService>();
            logger = Substitute.For<ILogger>();
            util = Substitute.For<IUtil>();
        }

        [Fact]
        public async Task PushTickets_WhenCalled_PushTickets()
        {
            // Arrange
            var configs = new ApiConfigs
            {
                Nova88 = new BrandApiConfigs
                {
                    TicketConfigs = new UrlApiConfig
                    {
                        QueryBatchSize = 1,
                        SyncBatchSize = 1
                    }
                }
            };
            apiConfigs.Value.Returns(configs);
            var service = new Nova88TicketService(
                apiConfigs,
                webClientService,
                dataService,
                logger,
                util);

            dataService.GetBetTickets(Arg.Any<string>(), Arg.Any<int>())
                .Returns(new List<BetTicket>());

            // Act
            await service.PushTickets();

            // Assert
            await dataService.Received().GetBetTickets(Arg.Any<string>(), Arg.Any<int>());
        }
    }
}
