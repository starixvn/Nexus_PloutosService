using Fanex.Logging;
using Microsoft.Extensions.Options;
using Ploutos.Common.Utils;
using Ploutos.GlobalComJobs.Models;
using Ploutos.GlobalComJobs.Services.Interfaces;
using System.Threading.Tasks;

namespace Ploutos.GlobalComJobs.Services
{
    public class L12BTicketService : SyncTicketBase, I12BTicketService
    {
        public L12BTicketService(IOptionsSnapshot<ApiConfigs> apiConfigs,
           IWebClientService webClientService,
           IDataService dataService,
           ILogger logger,
           IUtil util) : base(apiConfigs.Value.L12B, webClientService, dataService, logger, util)
        {
        }

        Task I12BTicketService.PushTickets()
        {
            return base.PushTickets();
        }
    }
}
