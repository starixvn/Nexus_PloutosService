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
    public class IvorybetTicketService : SyncTicketBase, IIvorybetTicketService
    {
        public IvorybetTicketService(IOptionsSnapshot<ApiConfigs> apiConfigs,
           IWebClientService webClientService,
           IDataService dataService,
           ILogger logger,
           IUtil util) : base(apiConfigs.Value.Ivorybet, webClientService, dataService, logger, util)
        {
        }

        Task<PushResult> IIvorybetTicketService.PushTickets()
        {
            return base.PushTickets();
        }
    }
}
