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

    public class SyncTicketBaseTests
    {
        private readonly IWebClientService webClientService;
        private readonly IDataService dataService;
        private readonly ILogger logger;
        private readonly IUtil util;

        public SyncTicketBaseTests()
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
            var syncTicketBase = new SyncTicketBase(apiConfigs, webClientService, dataService, logger, util);

            var betTickets = new List<BetTicket> {
                new BetTicket { TransactionDate = DateTime.Now, SettlementDate = DateTime.Now, TransactionNo = "1" }
            };
            dataService.GetBetTickets(Arg.Any<GetBetTicketCriteria>()).Returns(betTickets);
            util.SplitToBatches(Arg.Any<List<List<BetTicket>>>(), Arg.Any<int>())
                .Returns(new List<List<List<BetTicket>>> { new List<List<BetTicket>>
                    { betTickets, betTickets, betTickets, betTickets, betTickets }
                });

            var restResponse = new RestResponse
            {
                StatusCode = HttpStatusCode.OK,
                Content = "ac"
            };
            webClientService.PostAsync<List<BetTicket>>(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<List<BetTicket>>())
                .Returns(restResponse);

            var ticketResponse = new TicketResponse
            {
                Data = new List<TicketResponseData>
                {
                    new TicketResponseData{
                        ErroMessage = string.Empty,
                        TransactionNo = "1"
                    }
                }
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
        public async Task PushTickets_WhenStatusCodeBadRequest_UpdateTicketFailed()
        {
            // Arrange
            var apiConfigs = GetBrandConfigs();
            var syncTicketBase = new SyncTicketBase(apiConfigs, webClientService, dataService, logger, util);

            var betTickets = new List<BetTicket> { new BetTicket { TransactionDate = DateTime.Now, SettlementDate = DateTime.Now } };
            dataService.GetBetTickets(Arg.Any<GetBetTicketCriteria>()).Returns(betTickets);
            util.SplitToBatches(Arg.Any<List<List<BetTicket>>>(), Arg.Any<int>())
                .Returns(new List<List<List<BetTicket>>> { new List<List<BetTicket>> { betTickets } });

            var restResponse = new RestResponse
            {
                StatusCode = HttpStatusCode.BadRequest,
                Content = "ac"
            };
            webClientService.PostAsync<List<BetTicket>>(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<List<BetTicket>>())
                .Returns(restResponse);

            var ticketResponse = new TicketResponseBadRequest
            {
                ModelState = new Dictionary<string, string[]>
                {
                    { "criteria[0].Error1", new string[] {"Error1 Msg" } }
                }
            };
            util.ConvertJsonResponse<TicketResponseBadRequest>(Arg.Any<string>()).Returns(ticketResponse);

            List<UpdateBetTicket> updateTickets = null;
            await dataService.UpdateBetTickets(Arg.Any<string>(),
                Arg.Any<DateTime>(),
                Arg.Any<DateTime>(),
                Arg.Any<string>(),
                Arg.Do<List<UpdateBetTicket>>(x => updateTickets = x));

            // Act
            await syncTicketBase.PushTickets();

            // Assert
            updateTickets[0].IsError.Should().Be(true);
            updateTickets[0].ResponseResult.Should().Be("Error1 Msg");
        }

        [Fact]
        public async Task PushTickets_WhenStatusCodeOthers_UpdateTicketFailedAndWriteLog()
        {
            // Arrange
            var apiConfigs = GetBrandConfigs();
            var syncTicketBase = new SyncTicketBase(apiConfigs, webClientService, dataService, logger, util);

            var betTickets = new List<BetTicket> { new BetTicket { TransactionDate = DateTime.Now, SettlementDate = DateTime.Now } };
            dataService.GetBetTickets(Arg.Any<GetBetTicketCriteria>()).Returns(betTickets);
            util.SplitToBatches(Arg.Any<List<List<BetTicket>>>(), Arg.Any<int>())
                .Returns(new List<List<List<BetTicket>>> { new List<List<BetTicket>> { betTickets } });

            var restResponse = new RestResponse
            {
                StatusCode = HttpStatusCode.BadGateway,
                Content = "ac"
            };
            webClientService.PostAsync<List<BetTicket>>(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<List<BetTicket>>())
                .Returns(restResponse);

            var ticketResponse = new TicketResponseBadRequest
            {
                ModelState = new Dictionary<string, string[]>
                {
                    { "criteria[0].Error1", new string[] {"Error1 Msg" } }
                }
            };
            util.ConvertJsonResponse<TicketResponseBadRequest>(Arg.Any<string>()).Returns(ticketResponse);

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
                TicketConfigs = new TicketAPIConfig
                {
                    TotalRows = 1,
                    TotalHistoricalRows = 1,
                    FromNewlyCutoffDate = DateTime.Now,
                    ToHistoricalDate = DateTime.Now,
                    SyncBatchSize = 1,
                    SyncParallelSize = 5
                }
            };

            return brandConfigs;
        }
    }
}
