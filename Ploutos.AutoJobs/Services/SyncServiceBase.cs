namespace Ploutos.AutoJobs.Services
{
    using Fanex.Logging;
    using Ploutos.AutoJobs.Models;
    using Ploutos.AutoJobs.Utils;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class SyncServiceBase
    {
        private readonly BrandApiConfigs apiConfigs;
        private readonly IWebClientService webClientService;
        private readonly IDataService dataService;
        private readonly ILogger logger;
        private readonly IUtil util;

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
            var playerProfiles = await this.dataService.GetPlayerProfiles(siteName, apiConfigs.PlayerBatchSize);
            List<UpdatePlayerProfile> updatePlayerProfiles = new List<UpdatePlayerProfile>();

            foreach (var profile in playerProfiles)
            {
                var updateProfile = new UpdatePlayerProfile(profile);

                try
                {
                    var response = await webClientService.PostAsync<PlayerProfile>(
                        apiConfigs.PloutosBaseDomain,
                        apiConfigs.RegisterPlayerUrl,
                        apiConfigs.PloutosToken,
                        profile);

                    var profileResponse = this.util.ConvertJsonResponse<ProfileResponse>(response.Content);
                    updateProfile.ResponseResult = response.Content;
                    updateProfile.IsError = !profileResponse.IsSuccess;
                    updateProfile.PlayerCode = profileResponse.IsSuccess ? profileResponse.Data.PlayerAccountNumber : string.Empty;                    
                }
                catch (Exception ex)
                {
                    var errorMsg = this.util.BuildLogMessage(ex);
                    updateProfile.ResponseResult = errorMsg;
                    updateProfile.IsError = true;
                    await this.logger.ErrorAsync(errorMsg);
                }

                updatePlayerProfiles.Add(updateProfile);
            }

            var result = await this.dataService.UpdatePlayerProfiles(siteName, updatePlayerProfiles);
        }

        private async Task PushTickets(string siteName)
        {
            var betTickets = await this.dataService.GetBetTickets(siteName, apiConfigs.TicketBatchSize);
            List<UpdateBetTicket> updateBetTickets = new List<UpdateBetTicket>();
            foreach (var ticket in betTickets)
            {
                updateBetTickets.Add(new UpdateBetTicket(ticket));
            }

            try
            {
                var response = await webClientService.PostAsync<List<BetTicket>>(
                        apiConfigs.PloutosBaseDomain,
                        apiConfigs.SendBetUrl,
                        apiConfigs.PloutosToken,
                        betTickets);

                var ticketResponse = this.util.ConvertJsonResponse<TicketResponse>(response.Content);

                for (var i = 0; i < updateBetTickets.Count; i++)
                {
                    var errorMsg = ticketResponse.GetErrorMessage(i);
                    updateBetTickets[i].IsError = !string.IsNullOrEmpty(errorMsg);
                    updateBetTickets[i].ResponseResult = errorMsg;
                }
            }
            catch (Exception ex)
            {
                var errorMsg = this.util.BuildLogMessage(ex);
                foreach (var updateTicket in updateBetTickets)
                {
                    updateTicket.ResponseResult = errorMsg;
                    updateTicket.IsError = true;
                }

                await this.logger.ErrorAsync(errorMsg);
            }

            await this.dataService.UpdateBetTickets(siteName, updateBetTickets);
        }
    }
}
