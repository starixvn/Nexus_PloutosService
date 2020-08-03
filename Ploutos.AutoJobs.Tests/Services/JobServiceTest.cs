namespace Ploutos.AutoJobs.Tests.Services
{
    using System;
    using System.Threading.Tasks;
    using Fanex.Logging;
    using Microsoft.Extensions.Options;
    using NSubstitute;
    using NSubstitute.ExceptionExtensions;
    using ONEbook.UM.Models;
    using ONEbook.UM.Services;
    using Ploutos.AutoJobs.Models;
    using Ploutos.AutoJobs.Services;
    using Ploutos.AutoJobs.Services.Interfaces;
    using Ploutos.Common.Models;
    using Ploutos.Common.Services;
    using Ploutos.Common.Utils;
    using Xunit;

    public class JobServiceTest
    {
        private readonly IOptionsSnapshot<ApiConfigs> apiConfigs;
        private readonly IOpal88PlayerService opal88PlayerService;
        private readonly IOpal88TicketService opal88TicketService;
        private readonly INova88PlayerService nova88PlayerService;
        private readonly INova88TicketService nova88TicketService;
        private readonly IIvorybetPlayerService ivorybetPlayerService;
        private readonly IIvorybetTicketService ivorybetTicketService;
        private readonly IOpal88TicketBySupportApiService opal88TicketBySupportApiService;
        private readonly INova88TicketBySupportApiService nova88TicketBySupportApiService;
        private readonly IIvorybetTicketBySupportApiService ivorybetTicketBySupportApiService;
        private readonly ILogger logger;
        private readonly IUtil util;
        private readonly IMaintenanceService maintenanceService;
        private readonly IJobRunningChecker jobRunningChecker;

        public JobServiceTest()
        {
            apiConfigs = Substitute.For<IOptionsSnapshot<ApiConfigs>>();
            opal88PlayerService = Substitute.For<IOpal88PlayerService>();
            opal88TicketService = Substitute.For<IOpal88TicketService>();
            nova88PlayerService = Substitute.For<INova88PlayerService>();
            nova88TicketService = Substitute.For<INova88TicketService>();
            ivorybetPlayerService = Substitute.For<IIvorybetPlayerService>();
            ivorybetTicketService = Substitute.For<IIvorybetTicketService>();
            opal88TicketBySupportApiService = Substitute.For<IOpal88TicketBySupportApiService>();
            nova88TicketBySupportApiService = Substitute.For<INova88TicketBySupportApiService>();
            ivorybetTicketBySupportApiService = Substitute.For<IIvorybetTicketBySupportApiService>();
            logger = Substitute.For<ILogger>();
            util = Substitute.For<IUtil>();
            maintenanceService = Substitute.For<IMaintenanceService>();
            jobRunningChecker = Substitute.For<IJobRunningChecker>();
        }

        [Fact]
        public async Task SyncOpal88Player_WhenCalled_SyncOpal88Player()
        {
            // Arrange
            var configs = new ApiConfigs
            {
                Opal88 = new BrandApiConfigs
                {
                    PlayerConfigs = new UrlApiConfig
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
            var service = new JobService(
                apiConfigs,
                logger,
                util,
                maintenanceService,
                opal88PlayerService,
                opal88TicketService,
                nova88PlayerService,
                nova88TicketService,
                ivorybetPlayerService,
                ivorybetTicketService,
                opal88TicketBySupportApiService,
                nova88TicketBySupportApiService,
                ivorybetTicketBySupportApiService,
                jobRunningChecker);

            opal88PlayerService.PushPlayers().Returns(new PushResult { SuccessCount = 1, FailedCount = 1 });

            // Act
            await service.SyncOpal88Player();

            // Assert
            await opal88PlayerService.Received().PushPlayers();
        }

        [Fact]
        public async Task SyncOpal88Player_WhenIsRunning_NotSyncOpal88Player()
        {
            // Arrange
            var configs = new ApiConfigs
            {
                Opal88 = new BrandApiConfigs
                {
                    PlayerConfigs = new UrlApiConfig
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
            var service = new JobService(
                apiConfigs,
                logger,
                util,
                maintenanceService,
                opal88PlayerService,
                opal88TicketService,
                nova88PlayerService,
                nova88TicketService,
                ivorybetPlayerService,
                ivorybetTicketService,
                opal88TicketBySupportApiService,
                nova88TicketBySupportApiService,
                ivorybetTicketBySupportApiService,
                jobRunningChecker);
            jobRunningChecker.IsRunning(Constant.Constants.SyncOpal88Player).Returns(true);

            // Act
            await service.SyncOpal88Player();

            // Assert
            await opal88PlayerService.DidNotReceive().PushPlayers();
        }

        [Fact]
        public async Task SyncOpal88Player_WhenNotEnabled_NotSyncOpal88Player()
        {
            // Arrange
            var configs = new ApiConfigs
            {
                Opal88 = new BrandApiConfigs
                {
                    PlayerConfigs = new UrlApiConfig
                    {
                        IsEnabled = false
                    }
                }
            };

            var maintenanceTime = new UnderMaintenanceTime()
            {
                IsUnderMaintenanceTime = false
            };
            maintenanceService.GetUnderMaintenanceTimeAsync().Returns(maintenanceTime);

            apiConfigs.Value.Returns(configs);
            var service = new JobService(
                apiConfigs,
                logger,
                util,
                maintenanceService,
                opal88PlayerService,
                opal88TicketService,
                nova88PlayerService,
                nova88TicketService,
                ivorybetPlayerService,
                ivorybetTicketService,
                opal88TicketBySupportApiService,
                nova88TicketBySupportApiService,
                ivorybetTicketBySupportApiService,
                jobRunningChecker);

            // Act
            await service.SyncOpal88Player();

            // Assert
            await opal88PlayerService.DidNotReceive().PushPlayers();
        }

        [Fact]
        public async Task SyncOpal88Player_WhenIsUnderMaintenanceTime_NotSyncOpal88Player()
        {
            // Arrange
            var configs = new ApiConfigs
            {
                Opal88 = new BrandApiConfigs
                {
                    PlayerConfigs = new UrlApiConfig
                    {
                        IsEnabled = true
                    }
                }
            };

            var maintenanceTime = new UnderMaintenanceTime()
            {
                IsUnderMaintenanceTime = true
            };
            maintenanceService.GetUnderMaintenanceTimeAsync().Returns(maintenanceTime);

            apiConfigs.Value.Returns(configs);
            var service = new JobService(
                apiConfigs,
                logger,
                util,
                maintenanceService,
                opal88PlayerService,
                opal88TicketService,
                nova88PlayerService,
                nova88TicketService,
                ivorybetPlayerService,
                ivorybetTicketService,
                opal88TicketBySupportApiService,
                nova88TicketBySupportApiService,
                ivorybetTicketBySupportApiService,
                jobRunningChecker);

            // Act
            await service.SyncOpal88Player();

            // Assert
            await opal88PlayerService.DidNotReceive().PushPlayers();
        }

        [Fact]
        public async Task SyncOpal88Player_WhenException_WriteLog()
        {
            // Arrange
            var configs = new ApiConfigs
            {
                Opal88 = new BrandApiConfigs
                {
                    PlayerConfigs = new UrlApiConfig
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
            var service = new JobService(
                apiConfigs,
                logger,
                util,
                maintenanceService,
                opal88PlayerService,
                opal88TicketService,
                nova88PlayerService,
                nova88TicketService,
                ivorybetPlayerService,
                ivorybetTicketService,
                opal88TicketBySupportApiService,
                nova88TicketBySupportApiService,
                ivorybetTicketBySupportApiService,
                jobRunningChecker);
            var errors = "";
            util.BuildLogMessage(Arg.Any<Exception>()).Returns(errors);
            opal88PlayerService.PushPlayers().Throws(new Exception());

            // Act & Assert
            try
            {
                await service.SyncOpal88Player();
            }
            catch (Exception ex)
            {
                await logger.Received().ErrorAsync(errors);
            }
        }

        [Fact]
        public async Task SyncOpal88Ticket_WhenCalled_SyncOpal88Ticket()
        {
            // Arrange
            var configs = new ApiConfigs
            {
                Opal88 = new BrandApiConfigs
                {
                    TicketConfigs = new TicketAPIConfig
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
            var service = new JobService(
                apiConfigs,
                logger,
                util,
                maintenanceService,
                opal88PlayerService,
                opal88TicketService,
                nova88PlayerService,
                nova88TicketService,
                ivorybetPlayerService,
                ivorybetTicketService,
                opal88TicketBySupportApiService,
                nova88TicketBySupportApiService,
                ivorybetTicketBySupportApiService,
                jobRunningChecker);

            opal88TicketService.PushTickets().Returns(new PushResult { SuccessCount = 1, FailedCount = 1 });

            // Act
            await service.SyncOpal88Ticket();

            // Assert
            await opal88TicketService.Received().PushTickets();
        }

        [Fact]
        public async Task SyncNova88Player_WhenCalled_SyncNova88Player()
        {
            // Arrange
            var configs = new ApiConfigs
            {
                Nova88 = new BrandApiConfigs
                {
                    PlayerConfigs = new UrlApiConfig
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
            var service = new JobService(
                apiConfigs,
                logger,
                util,
                maintenanceService,
                opal88PlayerService,
                opal88TicketService,
                nova88PlayerService,
                nova88TicketService,
                ivorybetPlayerService,
                ivorybetTicketService,
                opal88TicketBySupportApiService,
                nova88TicketBySupportApiService,
                ivorybetTicketBySupportApiService,
                jobRunningChecker);

            nova88PlayerService.PushPlayers().Returns(new PushResult { SuccessCount = 1, FailedCount = 1 });

            // Act
            await service.SyncNova88Player();

            // Assert
            await nova88PlayerService.Received().PushPlayers();
        }

        [Fact]
        public async Task SyncNova88Ticket_WhenCalled_SyncNova88Ticket()
        {
            // Arrange
            var configs = new ApiConfigs
            {
                Nova88 = new BrandApiConfigs
                {
                    TicketConfigs = new TicketAPIConfig
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
            var service = new JobService(
                apiConfigs,
                logger,
                util,
                maintenanceService,
                opal88PlayerService,
                opal88TicketService,
                nova88PlayerService,
                nova88TicketService,
                ivorybetPlayerService,
                ivorybetTicketService,
                opal88TicketBySupportApiService,
                nova88TicketBySupportApiService,
                ivorybetTicketBySupportApiService,
                jobRunningChecker);

            nova88TicketService.PushTickets().Returns(new PushResult { SuccessCount = 1, FailedCount = 1 });

            // Act
            await service.SyncNova88Ticket();

            // Assert
            await nova88TicketService.Received().PushTickets();
        }

        [Fact]
        public async Task SyncIvorybetPlayer_WhenCalled_SyncIvorybetPlayer()
        {
            // Arrange
            var configs = new ApiConfigs
            {
                Ivorybet = new BrandApiConfigs
                {
                    PlayerConfigs = new UrlApiConfig
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
            var service = new JobService(
                apiConfigs,
                logger,
                util,
                maintenanceService,
                opal88PlayerService,
                opal88TicketService,
                nova88PlayerService,
                nova88TicketService,
                ivorybetPlayerService,
                ivorybetTicketService,
                opal88TicketBySupportApiService,
                nova88TicketBySupportApiService,
                ivorybetTicketBySupportApiService,
                jobRunningChecker);

            ivorybetPlayerService.PushPlayers().Returns(new PushResult { SuccessCount = 1, FailedCount = 1 });

            // Act
            await service.SyncIvorybetPlayer();

            // Assert
            await ivorybetPlayerService.Received().PushPlayers();
        }

        [Fact]
        public async Task SyncIvorybetTicket_WhenCalled_SyncIvorybetTicket()
        {
            // Arrange
            var configs = new ApiConfigs
            {
                Ivorybet = new BrandApiConfigs
                {
                    TicketConfigs = new TicketAPIConfig
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
            var service = new JobService(
                apiConfigs,
                logger,
                util,
                maintenanceService,
                opal88PlayerService,
                opal88TicketService,
                nova88PlayerService,
                nova88TicketService,
                ivorybetPlayerService,
                ivorybetTicketService,
                opal88TicketBySupportApiService,
                nova88TicketBySupportApiService,
                ivorybetTicketBySupportApiService,
                jobRunningChecker);

            ivorybetTicketService.PushTickets().Returns(new PushResult { SuccessCount = 1, FailedCount = 1 });

            // Act
            await service.SyncIvorybetTicket();

            // Assert
            await ivorybetTicketService.Received().PushTickets();
        }
    }
}