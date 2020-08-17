namespace Ploutos.AutoJobs.Tests.Services
{
    using Fanex.Logging;
    using Microsoft.Extensions.Options;
    using NSubstitute;
    using Ploutos.AutoJobs.Criteria;
    using Ploutos.AutoJobs.Models;
    using Ploutos.AutoJobs.Services;
    using Ploutos.Common.Services;
    using Ploutos.Common.Utils;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Xunit;

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
                Nova88 = SyncTicketBaseTest.GetBrandConfigs()
            };
            apiConfigs.Value.Returns(configs);
            var service = new Nova88TicketService(
                apiConfigs,
                webClientService,
                dataService,
                logger,
                util);

            dataService.GetBetTickets(Arg.Any<GetBetTicketCriteria>())
                .Returns(new List<BetTicket>());
            util.SplitToBatches(Arg.Any<List<List<BetTicket>>>(), Arg.Any<int>())
                .Returns(new List<List<List<BetTicket>>>());

            // Act
            await service.PushTickets();

            // Assert
            await dataService.Received().GetBetTickets(Arg.Any<GetBetTicketCriteria>());
        }
    }
}
