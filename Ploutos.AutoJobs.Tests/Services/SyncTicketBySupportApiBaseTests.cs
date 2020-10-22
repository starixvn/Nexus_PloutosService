namespace Ploutos.AutoJobs.Tests.Services
{
    using Fanex.Logging;
    using FluentAssertions;
    using NSubstitute;
    using Ploutos.AutoJobs.Criteria;
    using Ploutos.AutoJobs.Models;
    using Ploutos.AutoJobs.Services;
    using Ploutos.Common.Services;
    using Ploutos.Common.Utils;
    using RestSharp;
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;
    using Xunit;

    public class SyncTicketBySupportApiBaseTests
    {
        private readonly IWebClientService webClientService;
        private readonly IDataService dataService;
        private readonly ILogger logger;
        private readonly IUtil util;

        public SyncTicketBySupportApiBaseTests()
        {
            webClientService = Substitute.For<IWebClientService>();
            dataService = Substitute.For<IDataService>();
            logger = Substitute.For<ILogger>();
            util = Substitute.For<IUtil>();
        }

        [Fact]
        public async Task PushTickets_WhenStatusCodeOK_UpdateTicketsSuccess()
        {
            // Arrange
            var apiConfigs = GetBrandConfigs();
            var syncTicketBase = new SyncTicketBySupportApiBase(apiConfigs, webClientService, dataService, logger, util);

            var betTickets = new List<BetTicket> { new BetTicket { TransactionDate = DateTime.Now } };
            dataService.GetBetTickets(Arg.Any<GetBetTicketCriteria>()).Returns(betTickets);
            util.SplitToBatches(Arg.Any<List<BetTicket>>(), Arg.Any<int>())
                .Returns(new List<List<BetTicket>> { betTickets });

            var restResponse = new RestResponse
            {
                StatusCode = HttpStatusCode.OK,
                Content = "Data:[]"
            };
            webClientService.PostAsync<List<BetTicket>>(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<List<BetTicket>>())
                .Returns(restResponse);

            var ticketResponse = new TicketResponse
            {
                Data = new List<TicketResponseData>()
            };
            util.ConvertJsonResponse<TicketResponse>(Arg.Any<string>()).Returns(ticketResponse);

            List<UpdateBetTicket> updateTickets = null;
            await dataService.UpdateBetTickets(Arg.Any<string>(),
                Arg.Any<DateTime>(),
                Arg.Any<DateTime>(),
                Arg.Any<string>(),
                Arg.Do<List<UpdateBetTicket>>(x => updateTickets = x));

            // Act
            await syncTicketBase.PushTickets();

            // Assert
            updateTickets[0].IsError.Should().Be(false);
            updateTickets[0].ResponseResult.Should().BeEmpty();
        }

        [Fact]
        public async Task PushTickets_WhenStatusCodeOthers_UpdateTicketFailedAndWriteLog()
        {
            // Arrange
            var apiConfigs = GetBrandConfigs();
            var syncTicketBase = new SyncTicketBySupportApiBase(apiConfigs, webClientService, dataService, logger, util);

            var betTickets = new List<BetTicket> { new BetTicket { TransactionDate = DateTime.Now } };
            dataService.GetBetTickets(Arg.Any<GetBetTicketCriteria>()).Returns(betTickets);
            util.SplitToBatches(Arg.Any<List<BetTicket>>(), Arg.Any<int>())
                .Returns(new List<List<BetTicket>> { betTickets });

            var restResponse = new RestResponse
            {
                StatusCode = HttpStatusCode.BadGateway,
                Content = "ac"
            };
            webClientService.PostAsync<List<BetTicket>>(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<List<BetTicket>>())
                .Returns(restResponse);
           
            List<UpdateBetTicket> updateTickets = null;
            await dataService.UpdateBetTickets(Arg.Any<string>(),
                Arg.Any<DateTime>(),
                Arg.Any<DateTime>(),
                Arg.Any<string>(),
                Arg.Do<List<UpdateBetTicket>>(x => updateTickets = x));

            var errorMsg = "error msg";
            util.BuildLogMessage(Arg.Any<Exception>()).Returns(errorMsg);

            // Act
            await syncTicketBase.PushTickets();

            // Assert
            updateTickets[0].IsError.Should().Be(true);
            updateTickets[0].ResponseResult.Should().BeEmpty();
            await logger.Received().ErrorAsync(errorMsg);
        }

        public static BrandApiConfigs GetBrandConfigs()
        {
            var brandConfigs = new BrandApiConfigs
            {
                SupportApiConfigs = new SupportApiConfigs { },
                SupportTicketConfigs = new TicketAPIConfig { TotalRows = 1, SyncBatchSize = 1, TotalHistoricalRows = 1 }
            };

            return brandConfigs;
        }
    }
}
