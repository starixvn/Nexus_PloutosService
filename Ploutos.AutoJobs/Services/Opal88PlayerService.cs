namespace Ploutos.AutoJobs.Services
{
    using Fanex.Logging;
    using Microsoft.Extensions.Options;
    using Ploutos.AutoJobs.Models;
    using Ploutos.AutoJobs.Services.Interfaces;
    using Ploutos.AutoJobs.Utils;
    using System.Threading.Tasks;

    public class Opal88PlayerService : SyncPlayerBase, IOpal88PlayerService
    {
        public Opal88PlayerService(IOptionsSnapshot<ApiConfigs> apiConfigs,
           IWebClientService webClientService,
           IDataService dataService,
           ILogger logger,
           IUtil util) 
            : base(apiConfigs.Value.Opal88, webClientService, dataService, logger, util)
        {
        }      

        Task IOpal88PlayerService.PushPlayers()
        {
            return base.PushPlayers();
        }
    }
}
