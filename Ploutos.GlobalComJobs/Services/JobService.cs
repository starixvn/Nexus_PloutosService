using Fanex.Logging;
using Hangfire;
using Microsoft.Extensions.Options;
using ONEbook.UM.Services;
using Ploutos.Common.Services;
using Ploutos.Common.Utils;
using Ploutos.GlobalComJobs.Constant;
using Ploutos.GlobalComJobs.Models;
using Ploutos.GlobalComJobs.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace Ploutos.GlobalComJobs.Services
{
#pragma warning disable S107
    public class JobService : JobServiceBase, IJobService
    {
        private readonly IOptionsSnapshot<ApiConfigs> apiConfigs;
        private readonly I12BTicketService l12BTicketService;
        
        public JobService(
            IOptionsSnapshot<ApiConfigs> apiConfigs,
            ILogger logger,
            IUtil util,
            IMaintenanceService maintenanceService,
            I12BTicketService l12BTicketService,
            IJobRunningChecker jobRunningChecker) : base(logger, util, maintenanceService, jobRunningChecker)
        {
            this.apiConfigs = apiConfigs;           
            this.l12BTicketService = l12BTicketService;          
        }

        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
        public Task Sync12BTicket()
        {
            return SyncJob(apiConfigs.Value.L12B.TicketConfigs.IsEnabled, Constants.Sync12BTicket, l12BTicketService.PushTickets);
        }
    }
}
