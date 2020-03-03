using Fanex.Logging;
using ONEbook.UM.Services;
using Ploutos.Common.Utils;
using System;
using System.Threading.Tasks;

namespace Ploutos.Common.Services
{
    public class JobServiceBase
    {
        private readonly ILogger logger;
        private readonly IUtil util;
        private readonly IMaintenanceService maintenanceService;
        private readonly IJobRunningChecker jobRunningChecker;
        private readonly NLog.Logger fileLogger = NLog.LogManager.GetCurrentClassLogger();

        public JobServiceBase(
            ILogger logger,
            IUtil util,
            IMaintenanceService maintenanceService,
            IJobRunningChecker jobRunningChecker)
        {
            this.logger = logger;
            this.util = util;
            this.maintenanceService = maintenanceService;
            this.jobRunningChecker = jobRunningChecker;
        }

        protected async Task SyncJob(bool isEnabled, string jobName, Func<Task> fn)
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
                        fileLogger.Warn(string.Format("error syncjob {0} {1}", jobName, ex.Message));
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
