using System.Threading.Tasks;
using Fanex.Logging;
using Microsoft.Extensions.Options;
using Ploutos.AutoJobs.Models;
using Ploutos.AutoJobs.Services.Interfaces;
using Ploutos.Common.Models;
using Ploutos.Common.Services;
using Ploutos.Common.Utils;

namespace Ploutos.AutoJobs.Services
{
    public class Opal88TicketBySupportApiService : SyncTicketBySupportApiBase, IOpal88TicketBySupportApiService
    {
        public Opal88TicketBySupportApiService(IOptionsSnapshot<ApiConfigs> apiConfigs,
           IWebClientService webClientService,
           IDataService dataService,
           ILogger logger,
           IUtil util) : base(apiConfigs.Value.Opal88, webClientService, dataService, logger, util)
        {
        }

        Task<PushResult> IOpal88TicketBySupportApiService.PushTickets()
        {
            return base.PushTickets();
        }
    }
}
