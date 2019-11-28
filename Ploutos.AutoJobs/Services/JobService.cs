namespace Ploutos.AutoJobs.Services
{
    using Fanex.Logging;
    using Hangfire;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Options;
    using ONEbook.UM.Services;
    using Ploutos.AutoJobs.Models;
    using Ploutos.AutoJobs.Utils;
    using System;
    using System.IO;
    using System.Threading.Tasks;

    public class JobService : IJobService
    {
        private readonly IOptionsSnapshot<ApiConfigs> apiConfigs;
        private readonly INova88Service nova88Service;
        private readonly IIvorybetService ivorybetService;
        private readonly IOpal88Service opal88Service;
        private readonly ILogger logger;
        private readonly IUtil util;
        private readonly IMaintenanceService maintenanceService;

        public JobService(
            IOptionsSnapshot<ApiConfigs> apiConfigs,
            INova88Service nova88Service,
            IIvorybetService ivorybetService,
            IOpal88Service opal88Service,
            ILogger logger,
            IUtil util,
            IMaintenanceService maintenanceService)
        {
            this.apiConfigs = apiConfigs;
            this.nova88Service = nova88Service;
            this.ivorybetService = ivorybetService;
            this.opal88Service = opal88Service;
            this.logger = logger;
            this.util = util;
            this.maintenanceService = maintenanceService;
        }

        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
        public async Task SyncIvorybet()
        {
            bool isMaintenance = await maintenanceService.IsUnderMaintenanceAsync();
            if (apiConfigs.Value.Ivorybet.IsEnabled && !isMaintenance)
            {
                try
                {
                    await ivorybetService.PushData();
                }
                catch (Exception ex)
                {
                    await logger.ErrorAsync(this.util.BuildLogMessage(ex));
                    throw;
                }
            }
        }

        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
        public async Task SyncNova88()
        {
            bool isMaintenance = await maintenanceService.IsUnderMaintenanceAsync();
            if (apiConfigs.Value.Nova88.IsEnabled && !isMaintenance)
            {
                try
                {
                    await nova88Service.PushData();
                }
                catch (Exception ex)
                {
                    await logger.ErrorAsync(this.util.BuildLogMessage(ex));
                    throw;
                }
            }
        }

        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
        public async Task SyncOpal88()
        {
            bool isMaintenance = await maintenanceService.IsUnderMaintenanceAsync();
            if (apiConfigs.Value.Opal88.IsEnabled && !isMaintenance)
            {
                try
                {
                    await opal88Service.PushData();
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