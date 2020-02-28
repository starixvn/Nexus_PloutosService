using Hangfire;
using Hangfire.Storage;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.WindowsServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ploutos.GlobalComJobs.Resources;
using Ploutos.GlobalComJobs.Services.Interfaces;
using System;
using System.ComponentModel;
using System.Linq;

namespace Ploutos.GlobalComJobs.App_Configs
{
    public class CustomWebHostService : WebHostService
    {
        private readonly ILogger logger;
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
            logger.LogInformation(string.Format(Labels.Log_OnStartedService, DateTime.Now));
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
            logger.LogInformation(string.Format(Labels.Log_OnStoppingService, DateTime.Now));
            RemoveCurrentHangfireServer();
            base.OnStopping();
        }

        public void RunHangfireJobs()
        {
            recurringJob.AddOrUpdate<IJobService>(Labels.Job_Sync12BTicket,
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
