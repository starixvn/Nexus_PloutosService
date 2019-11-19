namespace Ploutos.AutoJobs.Services
{
    using System;
    using System.Threading.Tasks;
    using Fanex.Logging;
    using Fanex.Logging.Extensions;
    using Fanex.Logging.Extensions.Database;
    using Hangfire;

    public class JobService : IJobService
    {
        private readonly INova88Service nova88Service;
        private readonly IIvorybetService ivorybetService;
        private readonly IOpal88Service opal88Service;
        private readonly ILogger logger;

        public JobService(
            INova88Service nova88Service,
            IIvorybetService ivorybetService,
            IOpal88Service opal88Service,
            ILogger logger)
        {
            this.nova88Service = nova88Service;
            this.ivorybetService = ivorybetService;
            this.opal88Service = opal88Service;
            this.logger = logger;
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
                await Logging(ex);
                throw;
            }
        }

        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
        public async Task SyncNova88()
        {
            try
            {
                await nova88Service.PushData();
            }
            catch (Exception ex)
            {
                await Logging(ex);
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
                await Logging(ex);
                throw;
            }
        }
        
        private Task Logging(Exception exception)
        {
            var logMessage = new LogMessage(exception.Message)
                        .AddSection("Database Info", DatabaseInfoGetters.GetDbExceptionInfo(exception))
                        .AddSection("Server Info", CommonInfoGetters.GetServerInfo())
                        .AddSection("Exception Info", CommonInfoGetters.GetExceptionInfo(exception))
                        .ToString();
            return logger.ErrorAsync(logMessage);
        }
    }
}