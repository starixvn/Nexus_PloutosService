namespace Ploutos.AutoJobs.Services
{
    using Fanex.Logging;
    using Microsoft.Extensions.Options;
    using Ploutos.AutoJobs.Models;
    using Ploutos.AutoJobs.Utils;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using RestSharp;
    using Ploutos.AutoJobs.Constant;

    public class SyncPlayerBase
    {
        private readonly BrandApiConfigs apiConfigs;
        private readonly IWebClientService webClientService;
        private readonly IDataService dataService;
        private readonly ILogger logger;
        private readonly IUtil util;
        private readonly NLog.Logger fileLogger = NLog.LogManager.GetCurrentClassLogger();
        private readonly IOptionsSnapshot<PloutosSupportApiConfigs> supportApiConfigs;

        public SyncPlayerBase(BrandApiConfigs apiConfigs,
            IWebClientService webClientService,
            IDataService dataService,
            ILogger logger,
            IUtil util,
            IOptionsSnapshot<PloutosSupportApiConfigs> supportApiConfigs)
        {
            this.apiConfigs = apiConfigs;
            this.webClientService = webClientService;
            this.dataService = dataService;
            this.logger = logger;
            this.util = util;
            this.supportApiConfigs = supportApiConfigs;
        }
        
        protected virtual async Task PushPlayers()
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
                    apiConfigs.PloutosBaseDomain,
                    apiConfigs.PlayerConfigs.Url,
                    apiConfigs.PloutosToken,
                    profile);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var profileResponse = this.util.ConvertJsonResponse<ProfileResponse>(response.Content);
                    profileResponse = await UpdateResponseFromSupportAPI(profileResponse, updateProfile.Username);
                    updateProfile.ResponseResult = response.Content;
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

        private async Task<ProfileResponse> UpdateResponseFromSupportAPI(ProfileResponse profileResponse, string userName)
        {
            if(!profileResponse.IsSuccess && 
                profileResponse.Messages.Errors.Count > 0 &&
                profileResponse.Messages.Errors[0].Code == "USER-ERROR-10002" &&
                profileResponse.Messages.Errors[0].Description == "Username already in use.")
            {
                try
                {
                    var authObj = new SupportApiAuthObj
                    {
                        username = supportApiConfigs.Value.AppCredentialId,
                        password = supportApiConfigs.Value.AppCredentialPassword
                    };
                    var authResponse = await webClientService.PostAsync<SupportApiAuthObj>(
                        supportApiConfigs.Value.Domain, supportApiConfigs.Value.AuthUrl, string.Empty, authObj);

                    if(authResponse.StatusCode == HttpStatusCode.OK)
                    {
                        var token = authResponse.Content.Replace("\"","");
                        var parameters = new List<Parameter> { new Parameter { Name = "username", Value = userName }};
                        var dataResponse = await webClientService.GetAsync(
                            supportApiConfigs.Value.Domain, supportApiConfigs.Value.GetUserAccountNoUrl, token, parameters);

                        if(dataResponse.StatusCode == HttpStatusCode.OK && dataResponse.Content != "null")
                        {
                            var playerCode = dataResponse.Content.Replace("\"", "");
                            profileResponse.Status = Constants.StatusSuccess;
                            profileResponse.Data = new ProfileResponseData { PlayerAccountNumber = playerCode };
                        }
                    }
                }
                catch (Exception ex)
                {
                    await this.logger.ErrorAsync(this.util.BuildLogMessage(ex));
                }
            }

            return profileResponse;
        }
    }
}