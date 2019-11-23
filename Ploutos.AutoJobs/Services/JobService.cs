namespace Ploutos.AutoJobs.Services
{
    using System;
    using System.Threading.Tasks;
    using Fanex.Logging;
    using Fanex.Logging.Extensions;
    using Fanex.Logging.Extensions.Database;
    using Hangfire;
    using Ploutos.AutoJobs.Utils;

    public class JobService : IJobService
    {
        private readonly INova88Service nova88Service;
        private readonly IIvorybetService ivorybetService;
        private readonly IOpal88Service opal88Service;
        private readonly ILogger logger;
        private readonly IUtil util;
           

        public JobService(
            INova88Service nova88Service,
            IIvorybetService ivorybetService,
            IOpal88Service opal88Service,
            ILogger logger,
            IUtil util)
        {
            this.nova88Service = nova88Service;
            this.ivorybetService = ivorybetService;
            this.opal88Service = opal88Service;
            this.logger = logger;
            this.util = util;
        }

        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
        public async Task SyncIvorybet()
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

        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
        public async Task SyncNova88()
        {
            try
            {
                //await nova88Service.PushData();
            }
            catch (Exception ex)
            {
                await logger.ErrorAsync(this.util.BuildLogMessage(ex));
                throw;
            }
        }

        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
        public async Task SyncOpal88()
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