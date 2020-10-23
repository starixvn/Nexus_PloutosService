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
    public class Nova88TicketService : SyncTicketBase, INova88TicketService
    {
        public Nova88TicketService(IOptionsSnapshot<ApiConfigs> apiConfigs,
           IWebClientService webClientService,
           IDataService dataService,
           ILogger logger,
           IUtil util) : base(apiConfigs.Value.Nova88, webClientService, dataService, logger, util)
        {
        }

        public override async Task<PushResult> PushTickets()
        {
            return await base.PushTickets();
        }
    }
}
