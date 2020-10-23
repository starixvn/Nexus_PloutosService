namespace Ploutos.AutoJobs.Services
{
    using System.Threading.Tasks;
    using Fanex.Logging;
    using Microsoft.Extensions.Options;
    using Ploutos.AutoJobs.Models;
    using Ploutos.AutoJobs.Services.Interfaces;
    using Ploutos.Common.Models;
    using Ploutos.Common.Services;
    using Ploutos.Common.Utils;

    public class Ticket12BService : SyncTicketBase, ITicket12BService
    {
        public Ticket12BService(IOptionsSnapshot<ApiConfigs> apiConfigs,
          IWebClientService webClientService,
          IDataService dataService,
          ILogger logger,
          IUtil util) : base(apiConfigs.Value.Brand12B, webClientService, dataService, logger, util)
        {
        }

        public override async Task<PushResult> PushTickets()
        {
            return await base.PushTickets();
        }
    }
}