namespace Ploutos.AutoJobs.Services
{
    using Fanex.Logging;
    using Microsoft.Extensions.Options;
    using Ploutos.AutoJobs.Constant;
    using Ploutos.AutoJobs.Models;
    using Ploutos.AutoJobs.Utils;
    using System.Threading.Tasks;

    public class Nova88Service : SyncServiceBase, INova88Service
    {
        public Nova88Service(IOptionsSnapshot<ApiConfigs> apiConfigs,
            IWebClientService webClientService,
            IDataService dataService,
            ILogger logger,
            IUtil util) : base(apiConfigs.Value.Nova88, webClientService, dataService, logger, util)
        {
        }

        public Task PushData()
        {
            return base.PushData(Constants.Nova88);
        }
    }
}
