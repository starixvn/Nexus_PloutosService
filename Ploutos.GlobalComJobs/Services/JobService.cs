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
        private readonly IOpal88TicketService opal88TicketService;
        private readonly INova88TicketService nova88TicketService;
        private readonly IIvorybetTicketService ivorybetTicketService;

        public JobService(
            IOptionsSnapshot<ApiConfigs> apiConfigs,
            ILogger logger,
            IUtil util,
            IMaintenanceService maintenanceService,
            I12BTicketService l12BTicketService,
            IOpal88TicketService opal88TicketService,
            INova88TicketService nova88TicketService,
            IIvorybetTicketService ivorybetTicketService,
            IJobRunningChecker jobRunningChecker) : base(logger, util, maintenanceService, jobRunningChecker)
        {
            this.apiConfigs = apiConfigs;           
            this.l12BTicketService = l12BTicketService;
            this.opal88TicketService = opal88TicketService;
            this.nova88TicketService = nova88TicketService;
            this.ivorybetTicketService = ivorybetTicketService;
        }

        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
        public Task Sync12BTicket()
        {
            return SyncJob(apiConfigs.Value.L12B.TicketConfigs.IsEnabled, Constants.Sync12BTicket, l12BTicketService.PushTickets);
        }

        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
        public Task SyncIvorybetTicket()
        {
            return SyncJob(apiConfigs.Value.Ivorybet.TicketConfigs.IsEnabled, Constants.SyncIvorybetTicket, ivorybetTicketService.PushTickets);
        }

        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
        public Task SyncNova88Ticket()
        {
            return SyncJob(apiConfigs.Value.Nova88.TicketConfigs.IsEnabled, Constants.SyncNova88Ticket, nova88TicketService.PushTickets);
        }

        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
        public Task SyncOpal88Ticket()
        {
            return SyncJob(apiConfigs.Value.Opal88.TicketConfigs.IsEnabled, Constants.SyncOpal88Ticket, opal88TicketService.PushTickets);
        }
    }
}
