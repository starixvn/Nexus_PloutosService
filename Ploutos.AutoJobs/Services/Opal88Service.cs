namespace Ploutos.AutoJobs.Services
{
    using Microsoft.Extensions.Options;
    using Ploutos.AutoJobs.Criteria;
    using Ploutos.AutoJobs.Models;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class Opal88Service : IOpal88Service
    {
        private readonly ApiConfigs apiConfigs;
        private readonly IWebClientService webClientService;
        private readonly IDataService dataService;

        public Opal88Service(IOptionsSnapshot<ApiConfigs> apiConfigs, 
            IWebClientService webClientService,
            IDataService dataService)
        {
            this.apiConfigs = apiConfigs.Value;
            this.webClientService = webClientService;
            this.dataService = dataService;
        }

        public async Task PushData()
        {
            //await PushPlayers();
            await PushTickets();
        }

        private async Task PushPlayers()
        {
            var playerProfiles = await this.dataService.GetPlayerProfiles(apiConfigs.BatchSize);

            var start = DateTime.Now;

            foreach(var profile in playerProfiles)
            {
                var response = await webClientService.PostAsync<PlayerProfile>(
                    apiConfigs.PloutosBaseDomain,
                    apiConfigs.RegisterPlayerUrl,
                    apiConfigs.PloutosToken,
                    profile);

                var criteria = new InsertPlayerProfileLogCriteria() {
                    ResponeResult = response.Content
                };
                //await this.dataService.InsertPlayerProfileLog(criteria);

            }

            var timespan = (DateTime.Now - start).TotalSeconds;
        }

        private async Task PushTickets()
        {
            var betTickets = this.dataService.GetBetTickets(apiConfigs.BatchSize).Result;
            var response = await webClientService.PostAsync<List<BetTicket>>(
                    apiConfigs.PloutosBaseDomain,
                    apiConfigs.SendBetUrl,
                    apiConfigs.PloutosToken,
                    betTickets);
            var criteria = new InsertBetTicketLogCriteria();
            //await this.dataService.InsertBetTicketLog(criteria);
        }
    }
}
