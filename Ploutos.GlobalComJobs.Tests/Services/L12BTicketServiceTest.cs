using Fanex.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using Ploutos.Common.Services;
using Ploutos.Common.Utils;
using Ploutos.GlobalComJobs.Models;
using Ploutos.GlobalComJobs.Services;
using Ploutos.GlobalComJobs.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Ploutos.GlobalComJobs.Tests.Services
{
    public class L12BTicketServiceTest
    {
        private readonly IOptionsSnapshot<ApiConfigs> apiConfigs;
        private readonly IWebClientService webClientService;
        private readonly IDataService dataService;
        private readonly ILogger logger;
        private readonly IUtil util;
        
        public L12BTicketServiceTest()
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
                L12B = new BrandApiConfigs
                {
                    TicketConfigs = new UrlApiConfig
                    {
                        QueryBatchSize = 1,
                        SyncBatchSize = 1
                    }
                }
            };
            apiConfigs.Value.Returns(configs);
            var service = new L12BTicketService(
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
