namespace Ploutos.AutoJobs.Services
{
    using System.Threading.Tasks;
    using Fanex.Logging;
    using Microsoft.Extensions.Options;
    using Ploutos.AutoJobs.Models;
    using Ploutos.AutoJobs.Services.Interfaces;
    using Ploutos.AutoJobs.Utils;

    public class Opal88TicketService : SyncTicketBase, IOpal88TicketService
    {
        public Opal88TicketService(IOptionsSnapshot<ApiConfigs> apiConfigs,
           IWebClientService webClientService,
           IDataService dataService,
           ILogger logger,
           IUtil util) : base(apiConfigs.Value.Opal88, webClientService, dataService, logger, util)
        {
        }

        Task IOpal88TicketService.PushTickets()
        {
            return base.PushTickets();
        }
    }
}