namespace Ploutos.AutoJobs.Services
{
    using System;
    using System.Threading.Tasks;
    using Fanex.Logging;
    using Hangfire;
    using Microsoft.Extensions.Options;
    using ONEbook.UM.Services;
    using Ploutos.AutoJobs.Constant;
    using Ploutos.AutoJobs.Models;
    using Ploutos.AutoJobs.Services.Interfaces;
    using Ploutos.AutoJobs.Utils;

    #pragma warning disable S107
    public class JobService : IJobService
    {
        private readonly IOptionsSnapshot<ApiConfigs> apiConfigs;
        private readonly IOpal88PlayerService opal88PlayerService;
        private readonly IOpal88TicketService opal88TicketService;
        private readonly IOpal88TicketBySupportApiService opal88TicketBySupportApiService;
        private readonly INova88PlayerService nova88PlayerService;
        private readonly INova88TicketService nova88TicketService;
        private readonly INova88TicketBySupportApiService nova88TicketBySupportApiService;
        private readonly IIvorybetPlayerService ivorybetPlayerService;
        private readonly IIvorybetTicketService ivorybetTicketService;
        private readonly IIvorybetTicketBySupportApiService ivorybetTicketBySupportApiService;
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
            IOpal88PlayerService opal88PlayerService,
            IOpal88TicketService opal88TicketService,
            INova88PlayerService nova88PlayerService,
            INova88TicketService nova88TicketService,
            IIvorybetPlayerService ivorybetPlayerService,
            IIvorybetTicketService ivorybetTicketService,
            IOpal88TicketBySupportApiService opal88TicketBySupportApiService,
            INova88TicketBySupportApiService nova88TicketBySupportApiService,
            IIvorybetTicketBySupportApiService ivorybetTicketBySupportApiService,
            IJobRunningChecker jobRunningChecker)
        {
            this.apiConfigs = apiConfigs;
            this.logger = logger;
            this.util = util;
            this.maintenanceService = maintenanceService;
            this.opal88PlayerService = opal88PlayerService;
            this.opal88TicketService = opal88TicketService;
            this.nova88PlayerService = nova88PlayerService;
            this.nova88TicketService = nova88TicketService;
            this.ivorybetPlayerService = ivorybetPlayerService;
            this.ivorybetTicketService = ivorybetTicketService;
            this.opal88TicketBySupportApiService = opal88TicketBySupportApiService;
            this.nova88TicketBySupportApiService = nova88TicketBySupportApiService;
            this.ivorybetTicketBySupportApiService = ivorybetTicketBySupportApiService;
            this.jobRunningChecker = jobRunningChecker;
        }

        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
        public Task SyncOpal88Player()
        {
            return SyncJob(apiConfigs.Value.Opal88.PlayerConfigs.IsEnabled, Constants.SyncOpal88Player, opal88PlayerService.PushPlayers);
        }

        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
        public Task SyncOpal88Ticket()
        {
            return SyncJob(apiConfigs.Value.Opal88.TicketConfigs.IsEnabled, Constants.SyncOpal88Ticket, opal88TicketService.PushTickets);
        }

        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
        public Task SyncOpal88TicketBySupportApi()
        {
            return SyncJob(!apiConfigs.Value.Opal88.TicketConfigs.IsEnabled && apiConfigs.Value.Opal88.SupportTicketConfigs.IsEnabled, 
                Constants.SyncOpal88TicketBySupportApi, opal88TicketBySupportApiService.PushTickets);
        }

        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
        public Task SyncNova88Player()
        {
            return SyncJob(apiConfigs.Value.Nova88.PlayerConfigs.IsEnabled, Constants.SyncNova88Player, nova88PlayerService.PushPlayers);
        }

        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
        public Task SyncNova88Ticket()
        {
            return SyncJob(apiConfigs.Value.Nova88.TicketConfigs.IsEnabled, Constants.SyncNova88Ticket, nova88TicketService.PushTickets);
        }

        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
        public Task SyncNova88TicketBySupportApi()
        {
            return SyncJob(!apiConfigs.Value.Nova88.TicketConfigs.IsEnabled && apiConfigs.Value.Nova88.SupportTicketConfigs.IsEnabled, 
                Constants.SyncNova88TicketBySupportApi, nova88TicketBySupportApiService.PushTickets);
        }

        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
        public Task SyncIvorybetPlayer()
        {
            return SyncJob(apiConfigs.Value.Ivorybet.PlayerConfigs.IsEnabled, Constants.SyncIvorybetPlayer, ivorybetPlayerService.PushPlayers);
        }

        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
        public Task SyncIvorybetTicket()
        {
            return SyncJob(apiConfigs.Value.Ivorybet.TicketConfigs.IsEnabled, Constants.SyncIvorybetTicket, ivorybetTicketService.PushTickets);
        }

        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
        public Task SyncIvorybetTicketBySupportApi()
        {
            return SyncJob(!apiConfigs.Value.Ivorybet.TicketConfigs.IsEnabled && apiConfigs.Value.Ivorybet.SupportTicketConfigs.IsEnabled, 
                Constants.SyncIvorybetTicketBySupportApi, ivorybetTicketBySupportApiService.PushTickets);
        }

        private async Task SyncJob(bool isEnabled, string jobName, Func<Task> fn)
        {
            fileLogger.Warn(string.Format("enter syncjob {0}", jobName));
            if (isEnabled && !(await maintenanceService.GetUnderMaintenanceTimeAsync()).IsUnderMaintenanceTime)
            {
                var isRunning = await jobRunningChecker.IsRunning(jobName);
                if (!isRunning)
                {
                    try
                    {
                        fileLogger.Warn(string.Format("enter running job {0}", jobName));
                        await jobRunningChecker.MarkRunning(jobName);
                        await fn();
                        await jobRunningChecker.ClearRunning(jobName);
                    }
                    catch (Exception ex)
                    {
                        await jobRunningChecker.ClearRunning(jobName);
                        await logger.ErrorAsync(util.BuildLogMessage(ex));
                        fileLogger.Warn(string.Format("error syncjob {0} {1}", jobName, util.BuildLogMessage(ex)));
                        throw;
                    }
                }
                else
                {
                    fileLogger.Warn(string.Format("Another job is running for {0}, Skipped", jobName));
                }

            }
            fileLogger.Warn(string.Format("leave syncjob {0}", jobName));
        }
    }
}