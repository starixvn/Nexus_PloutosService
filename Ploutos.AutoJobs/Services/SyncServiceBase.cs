namespace Ploutos.AutoJobs.Services
{
    using Fanex.Logging;
    using Ploutos.AutoJobs.Models;
    using Ploutos.AutoJobs.Utils;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;

    public class SyncServiceBase
    {
        private readonly BrandApiConfigs apiConfigs;
        private readonly IWebClientService webClientService;
        private readonly IDataService dataService;
        private readonly ILogger logger;
        private readonly IUtil util;
        private readonly NLog.Logger fileLogger = NLog.LogManager.GetCurrentClassLogger();

        public SyncServiceBase(BrandApiConfigs apiConfigs,
            IWebClientService webClientService,
            IDataService dataService,
            ILogger logger,
            IUtil util)
        {
            this.apiConfigs = apiConfigs;
            this.webClientService = webClientService;
            this.dataService = dataService;
            this.logger = logger;
            this.util = util;
            
        }

        protected virtual async Task PushData(string siteName)
        {
            await PushPlayers(siteName);
            await PushTickets(siteName);
        }

        private async Task PushPlayers(string siteName)
        {
            fileLogger.Warn(string.Format("Start Push Players for {0}", siteName));

            var playerProfiles = await this.dataService.GetPlayerProfiles(siteName, apiConfigs.PlayerBatchSize);
            var profileListBatches = playerProfiles
                    .Select((s, i) => new { Value = s, Index = i })
                    .GroupBy(x => x.Index / 2)
                    .Select(grp => grp.Select(x => x.Value).ToArray())
                    .ToList();

            var successCount = 0;
            var failedCount = 0;
            foreach(var batch in profileListBatches)
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
            var responseTime = this.util.GetCurrentTimeAtGMTMinus4();
            try
            {   
                var response = await webClientService.PostAsync<PlayerProfile>(
                    apiConfigs.PloutosBaseDomain,
                    apiConfigs.RegisterPlayerUrl,
                    apiConfigs.PloutosToken,
                    profile);
                responseTime = this.util.GetCurrentTimeAtGMTMinus4();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var profileResponse = this.util.ConvertJsonResponse<ProfileResponse>(response.Content);
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
                    throw new Exception(response.ErrorMessage ?? response.Content);
                }
            }
            catch (Exception ex)
            {
                updateProfile.ResponseResult = ex.Message;
                updateProfile.IsError = true;
                await this.logger.ErrorAsync(this.util.BuildLogMessage(ex));
            }
            updateProfile.PushTime = pushTime;
            updateProfile.ResponseTime = responseTime;

            await this.dataService.UpdatePlayerProfiles(siteName, new List<UpdatePlayerProfile> { updateProfile });

            return !updateProfile.IsError;
        }

        private async Task PushTickets(string siteName)
        {
            fileLogger.Warn(string.Format("Start Push Tickets for {0}", siteName));

            var betTickets = await this.dataService.GetBetTickets(siteName, apiConfigs.TicketBatchSize);
            var betTicketBatches = betTickets
                    .Select((s, i) => new { Value = s, Index = i })
                    .GroupBy(x => x.Index / 100)
                    .Select(grp => grp.Select(x => x.Value).ToList())
                    .ToList();

            var successCount = 0;
            foreach(var batch in betTicketBatches)
            {
                var batchResult = await PushTicketBatch(siteName, batch);
                successCount += batchResult;
            }

            fileLogger.Warn(string.Format("Finish Push Tickets for {0}, total: {1}, success: {2}, failed: {3}",
                siteName, betTickets.Count, successCount, betTickets.Count - successCount));
        }

        private async Task<int> PushTicketBatch(string siteName, List<BetTicket> betTickets)
        {
            List<UpdateBetTicket> updateBetTickets = betTickets.Select(t => new UpdateBetTicket(t)).ToList();
            var responseResult = string.Empty;
            var pushTime = this.util.GetCurrentTimeAtGMTMinus4();
            var responseTime = this.util.GetCurrentTimeAtGMTMinus4();
            try
            {
                var response = await webClientService.PostAsync<List<BetTicket>>(
                        apiConfigs.PloutosBaseDomain,
                        apiConfigs.SendBetUrl,
                        apiConfigs.PloutosToken,
                        betTickets);
                responseResult = response.Content;
                responseTime = this.util.GetCurrentTimeAtGMTMinus4();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var ticketResponse = this.util.ConvertJsonResponse<TicketResponse>(response.Content);
                    for (var i = 0; i < updateBetTickets.Count; i++)
                    {
                        var errorMsg = ticketResponse.GetErrorMessage(updateBetTickets[i].TransactionNo);
                        updateBetTickets[i].IsError = !string.IsNullOrEmpty(errorMsg);
                        updateBetTickets[i].ResponseResult = errorMsg;
                    }
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    var ticketResponse = this.util.ConvertJsonResponse<TicketResponseBadRequest>(response.Content);

                    for (var i = 0; i < updateBetTickets.Count; i++)
                    {
                        var errorMsg = ticketResponse.GetErrorMessage(i);
                        updateBetTickets[i].IsError = true;
                        updateBetTickets[i].ResponseResult = errorMsg;
                    }
                }               
                else
                {
                    throw new Exception(response.ErrorMessage ?? response.Content);                    
                }
            }
            catch (Exception ex)
            {
                responseResult = ex.Message;
                foreach (var updateTicket in updateBetTickets)
                {
                    updateTicket.ResponseResult = string.Empty;
                    updateTicket.IsError = true;
                }

                await this.logger.ErrorAsync(this.util.BuildLogMessage(ex));
            }

            await this.dataService.UpdateBetTickets(siteName, pushTime, responseTime, responseResult, updateBetTickets);

            return updateBetTickets.Count(t => !t.IsError);
        }
    }
}
