namespace Ploutos.AutoJobs.Services
{
    using Fanex.Logging;
    using Microsoft.Extensions.Options;
    using Ploutos.AutoJobs.Models;
    using Ploutos.AutoJobs.Services.Interfaces;
    using Ploutos.Common.Utils;
    using System.Threading.Tasks;

    public class IvorybetTicketService : SyncTicketBase, IIvorybetTicketService
    {
        public IvorybetTicketService(IOptionsSnapshot<ApiConfigs> apiConfigs,
          IWebClientService webClientService,
          IDataService dataService,
          ILogger logger,
          IUtil util) : base(apiConfigs.Value.Ivorybet, webClientService, dataService, logger, util)
        {
        }

        Task IIvorybetTicketService.PushTickets()
        {
            return base.PushTickets();
        }
    }
}
