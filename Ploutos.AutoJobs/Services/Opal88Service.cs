namespace Ploutos.AutoJobs.Services
{
    using Fanex.Logging;
    using Microsoft.Extensions.Options;
    using Ploutos.AutoJobs.Constant;
    using Ploutos.AutoJobs.Criteria;
    using Ploutos.AutoJobs.Models;
    using Ploutos.AutoJobs.Utils;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;

    public class Opal88Service : IOpal88Service
    {
        private readonly BrandApiConfigs apiConfigs;
        private readonly IWebClientService webClientService;
        private readonly IDataService dataService;
        private readonly ILogger logger;
        private readonly IUtil util;

        public Opal88Service(IOptionsSnapshot<ApiConfigs> apiConfigs, 
            IWebClientService webClientService,
            IDataService dataService,
            ILogger logger,
            IUtil util)
        {
            this.apiConfigs = apiConfigs.Value.Opal88;
            this.webClientService = webClientService;
            this.dataService = dataService;
            this.logger = logger;
            this.util = util;
        }

        public async Task PushData()
        {
            //await PushPlayers();
            await PushTickets();
        }

        private async Task PushPlayers()
        {
            var playerProfiles = await this.dataService.GetPlayerProfiles(Constants.Opal88, apiConfigs.PlayerBatchSize);
            List<UpdatePlayerProfile> updatePlayerProfiles = new List<UpdatePlayerProfile>();

            foreach(var profile in playerProfiles)
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
                    updateProfile.ResponeResult = response.Content;
                    updateProfile.IsError = !profileResponse.IsSuccess;
                    updateProfile.PlayerCode = profileResponse.IsSuccess ? profileResponse.Data.PlayerAccountNumber : string.Empty;
                }
                catch(Exception ex)
                {
                    var errorMsg = this.util.BuildLogMessage(ex);
                    updateProfile.ResponeResult = errorMsg;
                    await this.logger.ErrorAsync(errorMsg);
                }

                updatePlayerProfiles.Add(updateProfile);
            }
       
            await this.dataService.UpdatePlayerProfiles(Constants.Opal88, updatePlayerProfiles);
        }

        private async Task PushTickets()
        {
            var betTickets = await this.dataService.GetBetTickets(Constants.Opal88, apiConfigs.TicketBatchSize);
            List<UpdateBetTicket> updateBetTickets = new List<UpdateBetTicket>();
            foreach(var ticket in betTickets)
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

                for(var i = 0; i < updateBetTickets.Count; i++)
                {
                    var errorMsg = ticketResponse.GetErrorMessage(i);
                    updateBetTickets[i].IsError = !string.IsNullOrEmpty(errorMsg);
                    updateBetTickets[i].ResponeResult = errorMsg;
                }
            }
            catch(Exception ex)
            {
                var errorMsg = this.util.BuildLogMessage(ex);
                foreach(var updateTicket in updateBetTickets)
                {
                    updateTicket.ResponeResult = errorMsg;
                }
                
                await this.logger.ErrorAsync(errorMsg);
            }

            await this.dataService.UpdateBetTickets(Constants.Opal88, updateBetTickets);
        }
    }
}
