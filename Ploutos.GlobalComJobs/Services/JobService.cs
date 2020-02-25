using Fanex.Logging;
using Hangfire;
using Microsoft.Extensions.Options;
using ONEbook.UM.Services;
using Ploutos.Common.Utils;
using Ploutos.GlobalComJobs.Constant;
using Ploutos.GlobalComJobs.Models;
using Ploutos.GlobalComJobs.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace Ploutos.GlobalComJobs.Services
{
#pragma warning disable S107
    public class JobService : IJobService
    {
        private readonly IOptionsSnapshot<ApiConfigs> apiConfigs;
        private readonly I12BTicketService l12BTicketService;
        private readonly ILogger logger;
        private readonly IUtil util;
        private readonly IMaintenanceService maintenanceService;
        private readonly IJobRunningChecker jobRunningChecker;
        private readonly NLog.Logger fileLogger = NLog.LogManager.GetCurrentClassLogger();

        public JobService(
            IOptionsSnapshot<ApiConfigs> apiConfigs,
            ILogger logger,
            IUtil util,
            IMaintenanceService maintenanceService,
            I12BTicketService l12BTicketService,
            IJobRunningChecker jobRunningChecker)
        {
            this.apiConfigs = apiConfigs;
            this.logger = logger;
            this.util = util;
            this.maintenanceService = maintenanceService;
            this.l12BTicketService = l12BTicketService;
            this.jobRunningChecker = jobRunningChecker;
        }

        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
        public Task Sync12BTicket()
        {
            return SyncJob(apiConfigs.Value.L12B.TicketConfigs.IsEnabled, Constants.Sync12BTicket, l12BTicketService.PushTickets);
        }

        private async Task SyncJob(bool isEnabled, string jobName, Func<Task> fn)
        {
            if (isEnabled && !(await maintenanceService.GetUnderMaintenanceTimeAsync()).IsUnderMaintenanceTime)
            {
                var isRunning = await jobRunningChecker.IsRunning(jobName);
                if (!isRunning)
                {
                    try
                    {
                        await jobRunningChecker.MarkRunning(jobName);
                        await fn();
                        await jobRunningChecker.ClearRunning(jobName);
                    }
                    catch (Exception ex)
                    {
                        await jobRunningChecker.ClearRunning(jobName);
                        await logger.ErrorAsync(util.BuildLogMessage(ex));
                        throw;
                    }
                }
                else
                {
                    fileLogger.Warn(string.Format("Another job is running for {0}, Skipped", jobName));
                }

            }
        }
    }
}
