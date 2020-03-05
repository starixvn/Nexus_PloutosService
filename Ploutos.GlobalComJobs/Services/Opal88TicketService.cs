using Fanex.Logging;
using Microsoft.Extensions.Options;
using Ploutos.Common.Models;
using Ploutos.Common.Services;
using Ploutos.Common.Utils;
using Ploutos.GlobalComJobs.Models;
using Ploutos.GlobalComJobs.Services.Interfaces;
using System.Threading.Tasks;

namespace Ploutos.GlobalComJobs.Services
{
    public class Opal88TicketService : SyncTicketBase, IOpal88TicketService
    {
        public Opal88TicketService(IOptionsSnapshot<ApiConfigs> apiConfigs,
           IWebClientService webClientService,
           IDataService dataService,
           ILogger logger,
           IUtil util) : base(apiConfigs.Value.Opal88, webClientService, dataService, logger, util)
        {
        }

        Task<PushResult> IOpal88TicketService.PushTickets()
        {
            return base.PushTickets();
        }
    }
}
