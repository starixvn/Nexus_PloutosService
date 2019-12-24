namespace Ploutos.AutoJobs.Services
{
    using Fanex.Logging;
    using Newtonsoft.Json;
    using Ploutos.AutoJobs.Models;
    using Ploutos.AutoJobs.Services.Interfaces;
    using Ploutos.AutoJobs.Utils;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class SyncPlayerBase
    {
        private readonly BrandApiConfigs apiConfigs;
        private readonly IWebClientService webClientService;
        private readonly IDataService dataService;
        private readonly ILogger logger;
        private readonly IUtil util;
        private readonly ISupportApiService supportApiService;
        private readonly NLog.Logger fileLogger = NLog.LogManager.GetCurrentClassLogger();

        public SyncPlayerBase(BrandApiConfigs apiConfigs,
            IWebClientService webClientService,
            IDataService dataService,
            ILogger logger,
            IUtil util,
            ISupportApiService supportApiService)
        {
            this.apiConfigs = apiConfigs;
            this.webClientService = webClientService;
            this.dataService = dataService;
            this.logger = logger;
            this.util = util;
            this.supportApiService = supportApiService;
        }
        
        public virtual async Task PushPlayers()
        {
            var siteName = this.apiConfigs.BrandName;
            fileLogger.Warn(string.Format("Start Push Players for {0}", siteName));

            var playerProfiles = await this.dataService.GetPlayerProfiles(siteName, apiConfigs.PlayerConfigs.QueryBatchSize);
            var profileListBatches = playerProfiles
                    .Select((s, i) => new { Value = s, Index = i })
                    .GroupBy(x => x.Index / apiConfigs.PlayerConfigs.SyncBatchSize)
                    .Select(grp => grp.Select(x => x.Value).ToArray())
                    .ToList();

            var successCount = 0;
            var failedCount = 0;
            foreach (var batch in profileListBatches)
            {
                var tasks = new Task<bool>[batch.Count()];
                for (int i = 0; i < batch.Count(); i++)
                {
                    tasks[i] = PushPlayer(batch[i], siteName);
                }

                var results = await Task.WhenAll(tasks);
                successCount += results.Count(r => r);
                failedCount += results.Count(r => !r);
            }

            fileLogger.Warn(string.Format("Finish Push Players for {0}, total: {1}, success: {2}, failed: {3}",
                siteName, playerProfiles.Count, successCount, failedCount));
        }

        private async Task<bool> PushPlayer(PlayerProfile profile, string siteName)
        {
            var updateProfile = new UpdatePlayerProfile(profile);
            var pushTime = this.util.GetCurrentTimeAtGMTMinus4();
            try
            {
                var response = await webClientService.PostAsync<PlayerProfile>(
                    apiConfigs.PloutosBaseDomain1,
                    apiConfigs.PlayerConfigs.Url,
                    apiConfigs.PloutosToken,
                    profile);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var profileResponse = this.util.ConvertJsonResponse<ProfileResponse>(response.Content);
                    profileResponse = await this.supportApiService.UpdateResponseFromSupportAPI(profileResponse, updateProfile.Username, apiConfigs.SupportApiConfigs);
                    updateProfile.ResponseResult = JsonConvert.SerializeObject(profileResponse);
                    updateProfile.IsError = !profileResponse.IsSuccess;
                    updateProfile.PlayerCode = profileResponse.IsSuccess ? profileResponse.Data.PlayerAccountNumber : string.Empty;
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    updateProfile.ResponseResult = response.Content;
                    updateProfile.IsError = true;
                }
                else
                {
                    throw new HttpRequestException(response.ErrorMessage ?? response.Content);
                }
            }
            catch (Exception ex)
            {
                updateProfile.ResponseResult = ex.Message;
                updateProfile.IsError = true;
                await this.logger.ErrorAsync(this.util.BuildLogMessage(ex));
            }
            updateProfile.PushTime = pushTime;
            updateProfile.ResponseTime = this.util.GetCurrentTimeAtGMTMinus4();

            await this.dataService.UpdatePlayerProfiles(siteName, new List<UpdatePlayerProfile> { updateProfile });

            return !updateProfile.IsError;
        }
    }
}