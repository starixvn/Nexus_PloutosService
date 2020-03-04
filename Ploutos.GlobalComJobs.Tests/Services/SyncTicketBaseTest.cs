using Fanex.Logging;
using FluentAssertions;
using NSubstitute;
using Ploutos.Common.Services;
using Ploutos.Common.Utils;
using Ploutos.GlobalComJobs.Models;
using Ploutos.GlobalComJobs.Services;
using Ploutos.GlobalComJobs.Services.Interfaces;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Ploutos.GlobalComJobs.Tests.Services
{
    public class SyncTicketBaseTest
    {
        private readonly IWebClientService webClientService;
        private readonly IDataService dataService;
        private readonly ILogger logger;
        private readonly IUtil util;

        public SyncTicketBaseTest()
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
                TicketConfigs = new UrlApiConfig { QueryBatchSize = 1, SyncBatchSize = 1 },
                AuthConfigs = new UrlApiConfig { }
            };
            var syncTicketBase = new SyncTicketBase(apiConfigs, webClientService, dataService, logger, util);

            var betTickets = new List<BetTicket> { new BetTicket { transaction_date = DateTime.Now, settlement_date = DateTime.Now } };
            dataService.GetBetTickets(Arg.Any<string>(), Arg.Any<int>()).Returns(betTickets);

            var restResponse = new RestResponse
            {
                StatusCode = HttpStatusCode.OK,
                Content = "ac"
            };
            webClientService.PostJsonAsync<ApiParam>(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<ApiParam>())
                .Returns(restResponse);

            var tokenResponse = new RestResponse
            {
                StatusCode = HttpStatusCode.OK,
                Content = string.Empty
            };
            webClientService.PostFormAsync<AuthParam>(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<AuthParam>())
                .Returns(tokenResponse);

            var ticketResponse = new TicketResponse
            {
                status = "posted"
            };
            util.ConvertJsonResponse<TicketResponse>(Arg.Any<string>()).Returns(ticketResponse);

            var authData = new AuthData { access_token = "abc" };
            util.ConvertJsonResponse<AuthData>(Arg.Any<string>()).Returns(authData);

            List<UpdateBetTicket> updateTickets = null;
            await dataService.UpdateBetTickets(Arg.Any<string>(),
                Arg.Any<DateTime>(),
                Arg.Any<DateTime>(),
                Arg.Any<string>(),
                Arg.Do<List<UpdateBetTicket>>(x => updateTickets = x));

            // Act
            await syncTicketBase.PushTickets();

            // Assert
            updateTickets[0].Is_Error.Should().Be(false);
            updateTickets[0].Response_Result.Should().BeEmpty();
        }

        [Fact]
        public async Task PushTickets_WhenStatusCodeUnauthorized_UpdateTicketFailed()
        {
            // Arrange
            var apiConfigs = new BrandApiConfigs
            {
                TicketConfigs = new UrlApiConfig { QueryBatchSize = 1, SyncBatchSize = 1 },
                AuthConfigs = new UrlApiConfig { }
            };
            var syncTicketBase = new SyncTicketBase(apiConfigs, webClientService, dataService, logger, util);

            var betTickets = new List<BetTicket> { new BetTicket { transaction_date = DateTime.Now, settlement_date = DateTime.Now } };
            dataService.GetBetTickets(Arg.Any<string>(), Arg.Any<int>()).Returns(betTickets);

            var restResponse = new RestResponse
            {
                StatusCode = HttpStatusCode.Unauthorized,
                Content = "ac"
            };
            webClientService.PostJsonAsync<ApiParam>(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<ApiParam>())
                .Returns(restResponse);

            var tokenResponse = new RestResponse
            {
                StatusCode = HttpStatusCode.OK,
                Content = string.Empty
            };
            webClientService.PostFormAsync<AuthParam>(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<AuthParam>())
                .Returns(tokenResponse);

            var ticketResponse = new TicketResponse
            {
                status = "posted"
            };
            util.ConvertJsonResponse<TicketResponse>(Arg.Any<string>()).Returns(ticketResponse);

            var authData = new AuthData { access_token = "abc" };
            util.ConvertJsonResponse<AuthData>(Arg.Any<string>()).Returns(authData);

            List<UpdateBetTicket> updateTickets = null;
            await dataService.UpdateBetTickets(Arg.Any<string>(),
                Arg.Any<DateTime>(),
                Arg.Any<DateTime>(),
                Arg.Any<string>(),
                Arg.Do<List<UpdateBetTicket>>(x => updateTickets = x));

            // Act
            await syncTicketBase.PushTickets();

            // Assert
            updateTickets[0].Is_Error.Should().Be(true);
            updateTickets[0].Response_Result.Should().BeEmpty();
        }

        [Fact]
        public async Task PushTickets_WhenStatusCodeBadRequest_UpdateTicketFailed()
        {
            // Arrange
            var apiConfigs = new BrandApiConfigs
            {
                TicketConfigs = new UrlApiConfig { QueryBatchSize = 1, SyncBatchSize = 1 },
                AuthConfigs = new UrlApiConfig { }
            };
            var syncTicketBase = new SyncTicketBase(apiConfigs, webClientService, dataService, logger, util);

            var betTickets = new List<BetTicket> {
                new BetTicket { transaction_date = DateTime.Now, settlement_date = DateTime.Now, transaction_id = "108091850837"
                } };
            dataService.GetBetTickets(Arg.Any<string>(), Arg.Any<int>()).Returns(betTickets);

            var restResponse = new RestResponse
            {
                StatusCode = HttpStatusCode.BadRequest,
                Content = string.Empty
            };
            webClientService.PostJsonAsync<ApiParam>(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<ApiParam>())
                .Returns(restResponse);

            var tokenResponse = new RestResponse
            {
                StatusCode = HttpStatusCode.OK,
                Content = string.Empty
            };
            webClientService.PostFormAsync<AuthParam>(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<AuthParam>())
                .Returns(tokenResponse);

            var ticketResponse = new TicketResponseBadRequest
            {
                Errors = new List<BadRequestObject>
                {
                   new BadRequestObject
                   {
                       Message = "Unsupported currency code",
                       RejectedValue = new BetTicket { transaction_id = "108091850837"}
                   }
                }
            };
            util.ConvertJsonResponse<TicketResponseBadRequest>(Arg.Any<string>()).Returns(ticketResponse);

            var authData = new AuthData { access_token = "abc" };
            util.ConvertJsonResponse<AuthData>(Arg.Any<string>()).Returns(authData);

            List<UpdateBetTicket> updateTickets = null;
            await dataService.UpdateBetTickets(Arg.Any<string>(),
                Arg.Any<DateTime>(),
                Arg.Any<DateTime>(),
                Arg.Any<string>(),
                Arg.Do<List<UpdateBetTicket>>(x => updateTickets = x));

            // Act
            await syncTicketBase.PushTickets();

            // Assert
            updateTickets[0].Is_Error.Should().Be(true);
            updateTickets[0].Response_Result.Should().Be("Unsupported currency code");
        }

        [Fact]
        public async Task PushTickets_WhenStatusCodeOthers_UpdateTicketFailedAndWriteLog()
        {
            // Arrange
            var apiConfigs = new BrandApiConfigs
            {
                TicketConfigs = new UrlApiConfig { QueryBatchSize = 1, SyncBatchSize = 1 },
                AuthConfigs = new UrlApiConfig { }
            };
            var syncTicketBase = new SyncTicketBase(apiConfigs, webClientService, dataService, logger, util);

            var betTickets = new List<BetTicket> { new BetTicket { transaction_date = DateTime.Now, settlement_date = DateTime.Now } };
            dataService.GetBetTickets(Arg.Any<string>(), Arg.Any<int>()).Returns(betTickets);

            var restResponse = new RestResponse
            {
                StatusCode = HttpStatusCode.BadGateway,
                Content = "ac"
            };
            webClientService.PostJsonAsync<ApiParam>(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<ApiParam>())
                .Returns(restResponse);

            var tokenResponse = new RestResponse
            {
                StatusCode = HttpStatusCode.OK,
                Content = string.Empty
            };
            webClientService.PostFormAsync<AuthParam>(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<AuthParam>())
                .Returns(tokenResponse);

            var ticketResponse = new TicketResponse
            {
                status = "posted"
            };
            util.ConvertJsonResponse<TicketResponse>(Arg.Any<string>()).Returns(ticketResponse);

            var authData = new AuthData { access_token = "abc" };
            util.ConvertJsonResponse<AuthData>(Arg.Any<string>()).Returns(authData);

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
            updateTickets[0].Is_Error.Should().Be(true);
            updateTickets[0].Response_Result.Should().BeEmpty();
            await logger.Received().ErrorAsync(errorMsg);
        }
    }
}
