namespace Ploutos.AutoJobs.Services
{
    using Fanex.Logging;
    using Microsoft.Extensions.Options;
    using Ploutos.AutoJobs.Models;
    using Ploutos.AutoJobs.Services.Interfaces;
    using Ploutos.Common.Models;
    using Ploutos.Common.Services;
    using Ploutos.Common.Utils;
    using System.Threading.Tasks;

    public class Nova88PlayerService : SyncPlayerBase, INova88PlayerService
    {
        public Nova88PlayerService(IOptionsSnapshot<ApiConfigs> apiConfigs,
          IWebClientService webClientService,
          IDataService dataService,
          ILogger logger,
          IUtil util,
          ISupportApiService supportApiService) 
            : base(apiConfigs.Value.Nova88, webClientService, dataService, logger, util, supportApiService)
        {
        }

        public override async Task<PushResult> PushPlayers()
        {
            return await base.PushPlayers();
        }
    }
}
