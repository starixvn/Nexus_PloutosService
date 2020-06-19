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

    public class Player12BService : SyncPlayerBase, IPlayer12BService
    {
        public Player12BService(IOptionsSnapshot<ApiConfigs> apiConfigs,
          IWebClientService webClientService,
          IDataService dataService,
          ILogger logger,
          IUtil util,
          ISupportApiService supportApiService) 
            : base(apiConfigs.Value.Brand12B, webClientService, dataService, logger, util, supportApiService)
        {
        }

        Task<PushResult> IPlayer12BService.PushPlayers()
        {
            return base.PushPlayers();
        }
    }
}
