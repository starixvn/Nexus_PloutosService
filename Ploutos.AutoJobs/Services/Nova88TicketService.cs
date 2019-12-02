namespace Ploutos.AutoJobs.Services
{
    using Fanex.Logging;
    using Microsoft.Extensions.Options;
    using Ploutos.AutoJobs.Models;
    using Ploutos.AutoJobs.Services.Interfaces;
    using Ploutos.AutoJobs.Utils;
    using System.Threading.Tasks;

    public class Nova88TicketService : SyncTicketBase, INova88TicketService
    {
        public Nova88TicketService(IOptionsSnapshot<ApiConfigs> apiConfigs,
          IWebClientService webClientService,
          IDataService dataService,
          ILogger logger,
          IUtil util) : base(apiConfigs.Value.Nova88, webClientService, dataService, logger, util)
        {
        }

        Task INova88TicketService.PushTickets()
        {
            return base.PushTickets();
        }
    }
}
