using Fanex.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using ONEbook.UM.Models;
using ONEbook.UM.Services;
using Ploutos.Common.Models;
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
    public class JobServiceTest
    {
        private readonly IOptionsSnapshot<ApiConfigs> apiConfigs;
        private readonly ILogger logger;
        private readonly IUtil util;
        private readonly IMaintenanceService maintenanceService;
        private readonly IJobRunningChecker jobRunningChecker;
        private readonly I12BTicketService l12BTicketService;
        private readonly IOpal88TicketService opal88TicketService;
        private readonly INova88TicketService nova88TicketService;
        private readonly IIvorybetTicketService ivorybetTicketService;

        public JobServiceTest()
        {
            apiConfigs = Substitute.For<IOptionsSnapshot<ApiConfigs>>();
            logger = Substitute.For<ILogger>();
            util = Substitute.For<IUtil>();
            maintenanceService = Substitute.For<IMaintenanceService>();
            jobRunningChecker = Substitute.For<IJobRunningChecker>();
            l12BTicketService = Substitute.For<I12BTicketService>();
            opal88TicketService = Substitute.For<IOpal88TicketService>();
            nova88TicketService = Substitute.For<INova88TicketService>();
            ivorybetTicketService = Substitute.For<IIvorybetTicketService>();
        }

        [Fact]
        public async Task Sync12BTicket_WhenCalled_Sync12BTicket()
        {
            // Arrange
            var configs = new ApiConfigs
            {
                L12B = new BrandApiConfigs
                {
                    TicketConfigs = new UrlApiConfig
                    {
                        IsEnabled = true
                    }
                }
            };

            var maintenanceTime = new UnderMaintenanceTime()
            {
                IsUnderMaintenanceTime = false
            };
            maintenanceService.GetUnderMaintenanceTimeAsync().Returns(maintenanceTime);

            apiConfigs.Value.Returns(configs);
            l12BTicketService.PushTickets().Returns(new PushResult { SuccessCount = 1, FailedCount = 1 });

            var service = new JobService(
                apiConfigs,
                logger,
                util,
                maintenanceService,
                l12BTicketService,
                opal88TicketService,
                nova88TicketService,
                ivorybetTicketService,
                jobRunningChecker);

            // Act
            await service.Sync12BTicket();

            // Assert
            await l12BTicketService.Received().PushTickets();
        }

        [Fact]
        public async Task Sync12BTicket_WhenIsRunning_NotSync12BTicket()
        {
            // Arrange
            var configs = new ApiConfigs
            {
                L12B = new BrandApiConfigs
                {
                    TicketConfigs = new UrlApiConfig
                    {
                        IsEnabled = true
                    }
                }
            };

            var maintenanceTime = new UnderMaintenanceTime()
            {
                IsUnderMaintenanceTime = false
            };
            maintenanceService.GetUnderMaintenanceTimeAsync().Returns(maintenanceTime);

            apiConfigs.Value.Returns(configs);

            jobRunningChecker.IsRunning(Constant.Constants.Sync12BTicket).Returns(true);

            var service = new JobService(
                apiConfigs,
                logger,
                util,
                maintenanceService,
                l12BTicketService,
                opal88TicketService,
                nova88TicketService,
                ivorybetTicketService,
                jobRunningChecker);

            // Act
            await service.Sync12BTicket();

            // Assert
            await l12BTicketService.DidNotReceive().PushTickets();
        }

        [Fact]
        public async Task SyncIvorybetTicket_WhenCalled_SyncIvorybetTicket()
        {
            // Arrange
            var configs = new ApiConfigs
            {
                Ivorybet = new BrandApiConfigs
                {
                    TicketConfigs = new UrlApiConfig
                    {
                        IsEnabled = true
                    }
                }
            };

            var maintenanceTime = new UnderMaintenanceTime()
            {
                IsUnderMaintenanceTime = false
            };
            maintenanceService.GetUnderMaintenanceTimeAsync().Returns(maintenanceTime);

            apiConfigs.Value.Returns(configs);
            ivorybetTicketService.PushTickets().Returns(new PushResult { SuccessCount = 1, FailedCount = 1 });

            var service = new JobService(
                apiConfigs,
                logger,
                util,
                maintenanceService,
                l12BTicketService,
                opal88TicketService,
                nova88TicketService,
                ivorybetTicketService,
                jobRunningChecker);

            // Act
            await service.SyncIvorybetTicket();

            // Assert
            await ivorybetTicketService.Received().PushTickets();
        }

        [Fact]
        public async Task SyncIvorybetTicket_WhenIsRunning_NotSyncIvorybetTicket()
        {
            // Arrange
            var configs = new ApiConfigs
            {
                Ivorybet = new BrandApiConfigs
                {
                    TicketConfigs = new UrlApiConfig
                    {
                        IsEnabled = true
                    }
                }
            };

            var maintenanceTime = new UnderMaintenanceTime()
            {
                IsUnderMaintenanceTime = false
            };
            maintenanceService.GetUnderMaintenanceTimeAsync().Returns(maintenanceTime);

            apiConfigs.Value.Returns(configs);

            jobRunningChecker.IsRunning(Constant.Constants.SyncIvorybetTicket).Returns(true);

            var service = new JobService(
                apiConfigs,
                logger,
                util,
                maintenanceService,
                l12BTicketService,
                opal88TicketService,
                nova88TicketService,
                ivorybetTicketService,
                jobRunningChecker);

            // Act
            await service.SyncIvorybetTicket();

            // Assert
            await ivorybetTicketService.DidNotReceive().PushTickets();
        }

        [Fact]
        public async Task SyncNova88Ticket_WhenCalled_SyncNova88Ticket()
        {
            // Arrange
            var configs = new ApiConfigs
            {
                Nova88 = new BrandApiConfigs
                {
                    TicketConfigs = new UrlApiConfig
                    {
                        IsEnabled = true
                    }
                }
            };

            var maintenanceTime = new UnderMaintenanceTime()
            {
                IsUnderMaintenanceTime = false
            };
            maintenanceService.GetUnderMaintenanceTimeAsync().Returns(maintenanceTime);

            apiConfigs.Value.Returns(configs);
            nova88TicketService.PushTickets().Returns(new PushResult { SuccessCount = 1, FailedCount = 1 });

            var service = new JobService(
                apiConfigs,
                logger,
                util,
                maintenanceService,
                l12BTicketService,
                opal88TicketService,
                nova88TicketService,
                ivorybetTicketService,
                jobRunningChecker);

            // Act
            await service.SyncNova88Ticket();

            // Assert
            await nova88TicketService.Received().PushTickets();
        }

        [Fact]
        public async Task SyncNova88Ticket_WhenIsRunning_NotSyncNova88Ticket()
        {
            // Arrange
            var configs = new ApiConfigs
            {
                Nova88 = new BrandApiConfigs
                {
                    TicketConfigs = new UrlApiConfig
                    {
                        IsEnabled = true
                    }
                }
            };

            var maintenanceTime = new UnderMaintenanceTime()
            {
                IsUnderMaintenanceTime = false
            };
            maintenanceService.GetUnderMaintenanceTimeAsync().Returns(maintenanceTime);

            apiConfigs.Value.Returns(configs);

            jobRunningChecker.IsRunning(Constant.Constants.SyncNova88Ticket).Returns(true);

            var service = new JobService(
                apiConfigs,
                logger,
                util,
                maintenanceService,
                l12BTicketService,
                opal88TicketService,
                nova88TicketService,
                ivorybetTicketService,
                jobRunningChecker);

            // Act
            await service.SyncNova88Ticket();

            // Assert
            await nova88TicketService.DidNotReceive().PushTickets();
        }

        [Fact]
        public async Task SyncOpal88Ticket_WhenCalled_SyncOpal88Ticket()
        {
            // Arrange
            var configs = new ApiConfigs
            {
                Opal88 = new BrandApiConfigs
                {
                    TicketConfigs = new UrlApiConfig
                    {
                        IsEnabled = true
                    }
                }
            };

            var maintenanceTime = new UnderMaintenanceTime()
            {
                IsUnderMaintenanceTime = false
            };
            maintenanceService.GetUnderMaintenanceTimeAsync().Returns(maintenanceTime);

            apiConfigs.Value.Returns(configs);
            opal88TicketService.PushTickets().Returns(new PushResult { SuccessCount = 1, FailedCount = 1 });

            var service = new JobService(
                apiConfigs,
                logger,
                util,
                maintenanceService,
                l12BTicketService,
                opal88TicketService,
                nova88TicketService,
                ivorybetTicketService,
                jobRunningChecker);

            // Act
            await service.SyncOpal88Ticket();

            // Assert
            await opal88TicketService.Received().PushTickets();
        }

        [Fact]
        public async Task SyncOpal88Ticket_WhenIsRunning_NotSyncOpal88Ticket()
        {
            // Arrange
            var configs = new ApiConfigs
            {
                Opal88 = new BrandApiConfigs
                {
                    TicketConfigs = new UrlApiConfig
                    {
                        IsEnabled = true
                    }
                }
            };

            var maintenanceTime = new UnderMaintenanceTime()
            {
                IsUnderMaintenanceTime = false
            };
            maintenanceService.GetUnderMaintenanceTimeAsync().Returns(maintenanceTime);

            apiConfigs.Value.Returns(configs);

            jobRunningChecker.IsRunning(Constant.Constants.SyncOpal88Ticket).Returns(true);

            var service = new JobService(
                apiConfigs,
                logger,
                util,
                maintenanceService,
                l12BTicketService,
                opal88TicketService,
                nova88TicketService,
                ivorybetTicketService,
                jobRunningChecker);

            // Act
            await service.SyncOpal88Ticket();

            // Assert
            await opal88TicketService.DidNotReceive().PushTickets();
        }
    }
}
