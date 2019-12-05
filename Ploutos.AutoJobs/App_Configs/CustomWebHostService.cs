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
    using Ploutos.AutoJobs.Services.Interfaces;
    using Resources;

    [DesignerCategory("Code")]
    public class CustomWebHostService : WebHostService
    {
        private readonly ILogger logger;
        private readonly IOptionsMonitor<CronJob> cronJob;
        private readonly IRecurringJobManager recurringJob;
        private readonly IJobRunningChecker jobRunningChecker;

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
            recurringJob.AddOrUpdate<IJobService>(Resources.Job_SyncOpal88Player,
                job => job.SyncOpal88Player(),
                cronJob.CurrentValue.SyncOpal88Player);
            recurringJob.AddOrUpdate<IJobService>(Resources.Job_SyncOpal88Ticket,
                job => job.SyncOpal88Ticket(),
                cronJob.CurrentValue.SyncOpal88Ticket);

            recurringJob.AddOrUpdate<IJobService>(Resources.Job_SyncNova88Player,
                job => job.SyncNova88Player(),
                cronJob.CurrentValue.SyncNova88Player);
            recurringJob.AddOrUpdate<IJobService>(Resources.Job_SyncNova88Ticket,
                job => job.SyncNova88Ticket(),
                cronJob.CurrentValue.SyncNova88Ticket);

            recurringJob.AddOrUpdate<IJobService>(Resources.Job_SyncIvorybetPlayer,
                job => job.SyncIvorybetPlayer(),
                cronJob.CurrentValue.SyncIvorybetPlayer);
            recurringJob.AddOrUpdate<IJobService>(Resources.Job_SyncIvorybetTicket,
                job => job.SyncIvorybetTicket(),
               cronJob.CurrentValue.SyncIvorybetTicket);
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