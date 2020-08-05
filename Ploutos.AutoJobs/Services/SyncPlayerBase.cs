namespace Ploutos.AutoJobs.Services
{
    using Fanex.Logging;
    using Newtonsoft.Json;
    using Ploutos.AutoJobs.Models;
    using Ploutos.AutoJobs.Services.Interfaces;
    using Ploutos.Common.Models;
    using Ploutos.Common.Services;
    using Ploutos.Common.Utils;
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
        
        public virtual async Task<PushResult> PushPlayers()
        {
            var siteName = this.apiConfigs.BrandName;

            var playerProfiles = await this.dataService.GetPlayerProfiles(siteName, apiConfigs.PlayerConfigs.QueryBatchSize);
            var profileListBatches = util.SplitToBatches<PlayerProfile>(playerProfiles, apiConfigs.PlayerConfigs.SyncBatchSize);

            if (siteName == "12Bet")
            {
                foreach (var player in playerProfiles)
                {
                    fileLogger.Warn(string.Format("exception post players {0}", player.Username));
                }
            }

            var successCount = 0;
            var failedCount = 0;
            foreach (var batch in profileListBatches)
            {
                var tasks = new Task<bool>[batch.Count];
                string domainName;
                for (int i = 0; i < batch.Count; i++)
                {
                    domainName = GetDomainName(i);
                    tasks[i] = PushPlayer(batch[i], siteName, domainName);
                }

                var results = await Task.WhenAll(tasks);
                successCount += results.Count(r => r);
                failedCount += results.Count(r => !r);
            }

            return new PushResult { SuccessCount = successCount, FailedCount = failedCount };
        }

        private string GetDomainName(int indexOfBatch)
        {
            string domainName;
            if (indexOfBatch % apiConfigs.PlayerConfigs.SyncBatchSize == 0)
            {
                domainName = apiConfigs.PloutosBaseDomain1;
            }
            else if (((indexOfBatch % apiConfigs.PlayerConfigs.SyncBatchSize) - 1) == 0)
            {
                domainName = apiConfigs.PloutosBaseDomain2;
            }
            else
            {
                domainName = apiConfigs.PloutosBaseDomain3;
            }

            return domainName;
        }

        private async Task<bool> PushPlayer(PlayerProfile profile, string siteName, string domainName)
        {
            var updateProfile = new UpdatePlayerProfile(profile);
            var pushTime = this.util.GetCurrentTimeAtGMTMinus4();
            try
            {
                var response = await webClientService.PostAsync<PlayerProfile>(
                    domainName,
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
                else if (response.StatusCode == HttpStatusCode.InternalServerError)
                {
                    var profileResponse = this.util.ConvertJsonResponse<ProfileResponse>(response.Content);
                    profileResponse = await this.supportApiService.UpdateResponse(profileResponse, updateProfile.Username, apiConfigs.SupportApiConfigs);
                    updateProfile.ResponseResult = JsonConvert.SerializeObject(profileResponse);
                    updateProfile.IsError = !profileResponse.IsSuccess;
                    updateProfile.PlayerCode = profileResponse.IsSuccess ? profileResponse.Data.PlayerAccountNumber : string.Empty;
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
                fileLogger.Warn(string.Format("exception post players {0} {1} {2} {3}", siteName, profile.Username, profile.CurrencyCode, ex.Message));
            }
            updateProfile.PushTime = pushTime;
            updateProfile.ResponseTime = this.util.GetCurrentTimeAtGMTMinus4();

            await this.dataService.UpdatePlayerProfiles(siteName, new List<UpdatePlayerProfile> { updateProfile });

            return !updateProfile.IsError;
        }
    }
}