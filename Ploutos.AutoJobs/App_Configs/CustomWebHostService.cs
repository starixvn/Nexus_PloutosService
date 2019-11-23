namespace Ploutos.AutoJobs.App_Configs
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using Hangfire;
    using Hangfire.Storage;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Hosting.WindowsServices;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Ploutos.AutoJobs.Services;
    using Resources;

    [DesignerCategory("Code")]
    public class CustomWebHostService : WebHostService
    {
        private readonly ILogger logger;
        private readonly IOptionsMonitor<CronJob> cronJob;
        private readonly IRecurringJobManager recurringJob;


        public CustomWebHostService(IWebHost host) : base(host)
        {
            logger = host.Services.GetService<ILogger<CustomWebHostService>>();
            cronJob = host.Services.GetService<IOptionsMonitor<CronJob>>();
            recurringJob = host.Services.GetService<IRecurringJobManager>();
        }

        protected override void OnStarted()
        {
            logger.LogInformation(string.Format(Resources.Log_OnStartedService, DateTime.Now));
            RunHangfireJobs();
            base.OnStarted();
        }

        protected override void OnStopping()
        {
            logger.LogInformation(string.Format(Resources.Log_OnStoppingService, DateTime.Now));
            RemoveCurrentHangfireServer();
            base.OnStopping();
        }

        public void RunHangfireJobs()
        {
            recurringJob.AddOrUpdate<IJobService>(Resources.Job_SyncNova88,
                job => job.SyncNova88(),
                cronJob.CurrentValue.SyncNova88);

            recurringJob.AddOrUpdate<IJobService>(Resources.Job_SyncIvorybet,
                job => job.SyncIvorybet(),
                cronJob.CurrentValue.SyncIvorybet);

            recurringJob.AddOrUpdate<IJobService>(Resources.Job_SyncOpal88,
                job => job.SyncOpal88(),
                cronJob.CurrentValue.SyncOpal88);
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