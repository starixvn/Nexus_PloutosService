using Fanex.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using ONEbook.UM.Models;
using ONEbook.UM.Services;
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

        public JobServiceTest()
        {
            apiConfigs = Substitute.For<IOptionsSnapshot<ApiConfigs>>();
            logger = Substitute.For<ILogger>();
            util = Substitute.For<IUtil>();
            maintenanceService = Substitute.For<IMaintenanceService>();
            jobRunningChecker = Substitute.For<IJobRunningChecker>();
            l12BTicketService = Substitute.For<I12BTicketService>();
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

            var service = new JobService(
                apiConfigs,
                logger,
                util,
                maintenanceService,
                l12BTicketService,
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
                jobRunningChecker);

            // Act
            await service.Sync12BTicket();

            // Assert
            await l12BTicketService.DidNotReceive().PushTickets();
        }
    }
}
