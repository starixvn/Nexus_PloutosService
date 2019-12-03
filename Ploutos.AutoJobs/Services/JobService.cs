namespace Ploutos.AutoJobs.Services
{
    using System;
    using System.Threading.Tasks;
    using Fanex.Logging;
    using Hangfire;
    using Microsoft.Extensions.Options;
    using ONEbook.UM.Services;
    using Ploutos.AutoJobs.Models;
    using Ploutos.AutoJobs.Services.Interfaces;
    using Ploutos.AutoJobs.Utils;

    public class JobService : IJobService
    {
        private readonly IOptionsSnapshot<ApiConfigs> apiConfigs;
        private readonly IOpal88PlayerService opal88PlayerService;
        private readonly IOpal88TicketService opal88TicketService;
        private readonly INova88PlayerService nova88PlayerService;
        private readonly INova88TicketService nova88TicketService;
        private readonly IIvorybetPlayerService ivorybetPlayerService;
        private readonly IIvorybetTicketService ivorybetTicketService;        
        private readonly ILogger logger;
        private readonly IUtil util;
        private readonly IMaintenanceService maintenanceService;

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
            IIvorybetTicketService ivorybetTicketService)
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
        }

        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
        public async Task SyncOpal88Player()
        {
            var maintanceTime = await maintenanceService.GetUnderMaintenanceTimeAsync();
            if (apiConfigs.Value.Opal88.PlayerConfigs.IsEnabled && !maintanceTime.IsUnderMaintenanceTime)
            {
                try
                {
                    await opal88PlayerService.PushPlayers();
                }
                catch (Exception ex)
                {
                    await logger.ErrorAsync(this.util.BuildLogMessage(ex));
                    throw;
                }
            }
        }

        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
        public async Task SyncOpal88Ticket()
        {
            var maintanceTime = await maintenanceService.GetUnderMaintenanceTimeAsync();
            if (apiConfigs.Value.Opal88.TicketConfigs.IsEnabled && !maintanceTime.IsUnderMaintenanceTime)
            {
                try
                {
                    await opal88TicketService.PushTickets();
                }
                catch (Exception ex)
                {
                    await logger.ErrorAsync(this.util.BuildLogMessage(ex));
                    throw;
                }
            }
        }

        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
        public async Task SyncNova88Player()
        {
            var maintanceTime = await maintenanceService.GetUnderMaintenanceTimeAsync();
            if (apiConfigs.Value.Nova88.PlayerConfigs.IsEnabled && !maintanceTime.IsUnderMaintenanceTime)
            {
                try
                {
                    await nova88PlayerService.PushPlayers();
                }
                catch (Exception ex)
                {
                    await logger.ErrorAsync(this.util.BuildLogMessage(ex));
                    throw;
                }
            }
        }

        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
        public async Task SyncNova88Ticket()
        {
            var maintanceTime = await maintenanceService.GetUnderMaintenanceTimeAsync();
            if (apiConfigs.Value.Nova88.TicketConfigs.IsEnabled && !maintanceTime.IsUnderMaintenanceTime)
            {
                try
                {
                    await nova88TicketService.PushTickets();
                }
                catch (Exception ex)
                {
                    await logger.ErrorAsync(this.util.BuildLogMessage(ex));
                    throw;
                }
            }
        }        

        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
        public async Task SyncIvorybetPlayer()
        {
            var maintanceTime = await maintenanceService.GetUnderMaintenanceTimeAsync();
            if (apiConfigs.Value.Ivorybet.PlayerConfigs.IsEnabled && !maintanceTime.IsUnderMaintenanceTime)
            {
                try
                {
                    await ivorybetPlayerService.PushPlayers();
                }
                catch (Exception ex)
                {
                    await logger.ErrorAsync(this.util.BuildLogMessage(ex));
                    throw;
                }
            }
        }

        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
        public async Task SyncIvorybetTicket()
        {
            var maintanceTime = await maintenanceService.GetUnderMaintenanceTimeAsync();
            if (apiConfigs.Value.Ivorybet.TicketConfigs.IsEnabled && !maintanceTime.IsUnderMaintenanceTime)
            {
                try
                {
                    await ivorybetTicketService.PushTickets();
                }
                catch (Exception ex)
                {
                    await logger.ErrorAsync(this.util.BuildLogMessage(ex));
                    throw;
                }
            }
        }
    }
}