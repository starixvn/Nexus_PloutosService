namespace Ploutos.AutoJobs.Services
{
    using Fanex.Logging;
    using Microsoft.Extensions.Options;
    using Ploutos.AutoJobs.Constant;
    using Ploutos.AutoJobs.Models;
    using Ploutos.AutoJobs.Utils;
    using System.Threading.Tasks;

    public class Opal88Service : SyncServiceBase, IOpal88Service
    {      
        public Opal88Service(IOptionsSnapshot<ApiConfigs> apiConfigs,
            IWebClientService webClientService,
            IDataService dataService,
            ILogger logger,
            IUtil util):base(apiConfigs.Value.Opal88, webClientService, dataService, logger, util)
        {
        }

        public Task PushData()
        {
            return base.PushData(Constants.Opal88);
        }
    }
}