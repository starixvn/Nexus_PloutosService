namespace Ploutos.AutoJobs.Services
{
    using Fanex.Logging;
    using Microsoft.Extensions.Options;
    using Ploutos.AutoJobs.Models;
    using Ploutos.AutoJobs.Services.Interfaces;
    using Ploutos.Common.Services;
    using Ploutos.Common.Utils;
    using System.Threading.Tasks;

    public class IvorybetPlayerService : SyncPlayerBase, IIvorybetPlayerService
    {
        public IvorybetPlayerService(IOptionsSnapshot<ApiConfigs> apiConfigs,
         IWebClientService webClientService,
         IDataService dataService,
         ILogger logger,
         IUtil util,
         ISupportApiService supportApiService) 
            : base(apiConfigs.Value.Ivorybet, webClientService, dataService, logger, util, supportApiService)
        {
        }

        Task IIvorybetPlayerService.PushPlayers()
        {
            return base.PushPlayers();
        }
    }
}
