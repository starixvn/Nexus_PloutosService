using Fanex.Logging;
using FluentAssertions;
using NSubstitute;
using Ploutos.AutoJobs.Models;
using Ploutos.AutoJobs.Services;
using Ploutos.AutoJobs.Utils;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Ploutos.AutoJobs.Tests.Services
{
    public class SyncTicketBySupportApiBaseTest
    {
        private readonly IWebClientService webClientService;
        private readonly IDataService dataService;
        private readonly ILogger logger;
        private readonly IUtil util;

        public SyncTicketBySupportApiBaseTest()
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
            var apiConfigs = new BrandApiConfigs
            {
                SupportApiConfigs = new SupportApiConfigs { },
                SupportTicketConfigs = new UrlApiConfig { QueryBatchSize = 1, SyncBatchSize = 1 }
            };
            var syncTicketBase = new SyncTicketBySupportApiBase(apiConfigs, webClientService, dataService, logger, util);

            var betTickets = new List<BetTicket> { new BetTicket { TransactionDate = DateTime.Now } };
            dataService.GetBetTickets(Arg.Any<string>(), Arg.Any<int>()).Returns(betTickets);

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
            var apiConfigs = new BrandApiConfigs
            {
                SupportApiConfigs = new SupportApiConfigs { },
                SupportTicketConfigs = new UrlApiConfig { QueryBatchSize = 1, SyncBatchSize = 1 }
            };
            var syncTicketBase = new SyncTicketBySupportApiBase(apiConfigs, webClientService, dataService, logger, util);

            var betTickets = new List<BetTicket> { new BetTicket { TransactionDate = DateTime.Now } };
            dataService.GetBetTickets(Arg.Any<string>(), Arg.Any<int>()).Returns(betTickets);

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
    }
}
