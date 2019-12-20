using Fanex.Logging;
using Microsoft.Extensions.Options;
using Ploutos.AutoJobs.Models;
using Ploutos.AutoJobs.Services.Interfaces;
using Ploutos.AutoJobs.Utils;
using System.Threading.Tasks;

namespace Ploutos.AutoJobs.Services
{
    public class Nova88TicketBySupportApiService : SyncTicketBySupportApiBase, INova88TicketBySupportApiService
    {
        public Nova88TicketBySupportApiService(IOptionsSnapshot<ApiConfigs> apiConfigs,
          IWebClientService webClientService,
          IDataService dataService,
          ILogger logger,
          IUtil util) : base(apiConfigs.Value.Nova88, webClientService, dataService, logger, util)
        {
        }

        Task INova88TicketBySupportApiService.PushTickets()
        {
            return base.PushTickets();
        }
    }
}
