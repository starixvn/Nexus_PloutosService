
namespace Ploutos.GlobalComJobs.Workers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Options;
    using ONEbook.UM.Services;
    using Ploutos.GlobalComJobs.Constant;
    using Ploutos.GlobalComJobs.Extensions;
    using Ploutos.GlobalComJobs.Models;
    using Ploutos.GlobalComJobs.Services.Interfaces;

    public class SendTicketIvorybetWorker : BaseWorker
    {
        private readonly IIvorybetTicketService ivorybetTicketService;
        
        public SendTicketIvorybetWorker(
            IIvorybetTicketService ivorybetTicketService,
            IMaintenanceService maintenanceService,
            IOptions<List<JobSetting>> jobSettings,
            ILogServiceExtension logService)
                : base(maintenanceService, logService)
        {
            this.ivorybetTicketService = ivorybetTicketService;
            jobSetting = JobHelper.GetJobConfig(jobSettings.Value, JobName.SendTicketIvorybet);
        }

        public override async Task ExecuteJobAsync()
        {
            await ivorybetTicketService.PushTickets();
        }
    }
}
