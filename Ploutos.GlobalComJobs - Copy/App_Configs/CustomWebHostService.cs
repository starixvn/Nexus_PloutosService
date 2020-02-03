namespace Ploutos.GlobalComJobs.App_Configs
{
    using Hangfire;
    using Hangfire.Storage;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Hosting.WindowsServices;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Ploutos.GlobalComJobs.Services.Interfaces;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.ComponentModel;
    using System.Linq;
    using Resources;

    public class CustomWebHostService : WebHostService
    {
        private readonly Microsoft.Extensions.Logging.ILogger logger;
        private readonly IOptionsMonitor<CronJob> cronJob;
        private readonly IRecurringJobManager recurringJob;
        private readonly IJobRunningChecker jobRunningChecker;

        [DesignerCategory("Code")]
        public CustomWebHostService(IWebHost host) : base(host)
        {
            logger = host.Services.GetService<ILogger<CustomWebHostService>>();
            cronJob = host.Services.GetService<IOptionsMonitor<CronJob>>();
            recurringJob = host.Services.GetService<IRecurringJobManager>();
            jobRunningChecker = host.Services.GetService<IJobRunningChecker>();
        }

        protected override void OnStarted()
        {
            jobRunningChecker.ClearAllRunning();
            logger.LogInformation(string.Format(Resources.Log_OnStartedService, DateTime.Now));
            RunHangfireJobs();
            base.OnStarted();
        }

        protected override void OnStopped()
        {
            logger.LogInformation(string.Format("Service Stopped at {0}", DateTime.Now));
            base.OnStopped();
        }

        protected override void OnStopping()
        {
            logger.LogInformation(string.Format(Resources.Log_OnStoppingService, DateTime.Now));
            RemoveCurrentHangfireServer();
            base.OnStopping();
        }

        public void RunHangfireJobs()
        {
            //var x = Ploutos.GlobalComJobs.Resources.
            recurringJob.AddOrUpdate<IJobService>("Resources.Job_Sync12BTicket",
                job => job.Sync12BTicket(),
                cronJob.CurrentValue.Sync12BTicket);
        }

        private static void RemoveCurrentHangfireServer()
        {
            IMonitoringApi monitoringApi = JobStorage.Current.GetMonitoringApi();
            var serverToRemove = monitoringApi.Servers().FirstOrDefault(s => s.Name.ToLowerInvariant().Contains(Environment.MachineName.ToLowerInvariant()));
            if (serverToRemove != null)
            {
                JobStorage.Current.GetConnection().RemoveServer(serverToRemove?.Name);
            }
        }
    }
}
