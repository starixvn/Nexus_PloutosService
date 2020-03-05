using Fanex.Logging;
using Microsoft.Extensions.Options;
using Ploutos.AutoJobs.Models;
using Ploutos.AutoJobs.Services.Interfaces;
using Ploutos.Common.Models;
using Ploutos.Common.Services;
using Ploutos.Common.Utils;
using System.Threading.Tasks;

namespace Ploutos.AutoJobs.Services
{
    public class IvorybetTicketBySupportApiService : SyncTicketBySupportApiBase, IIvorybetTicketBySupportApiService
    {
        public IvorybetTicketBySupportApiService(IOptionsSnapshot<ApiConfigs> apiConfigs,
          IWebClientService webClientService,
          IDataService dataService,
          ILogger logger,
          IUtil util) : base(apiConfigs.Value.Ivorybet, webClientService, dataService, logger, util)
        {
        }

        Task<PushResult> IIvorybetTicketBySupportApiService.PushTickets()
        {
            return base.PushTickets();
        }
    }
}
