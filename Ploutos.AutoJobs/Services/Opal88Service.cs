namespace Ploutos.AutoJobs.Services
{
    using Microsoft.Extensions.Options;
    using Ploutos.AutoJobs.Models;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class Opal88Service : IOpal88Service
    {
        private readonly ApiConfigs apiConfigs;
        private readonly IWebClientService webClientService;

        public Opal88Service(IOptionsSnapshot<ApiConfigs> apiConfigs, IWebClientService webClientService)
        {
            this.apiConfigs = apiConfigs.Value;
            this.webClientService = webClientService;
        }

        public async Task PushData()
        {
            var playerProfile = new PlayerProfile();
            await webClientService.PostAsync<PlayerProfile>(
                apiConfigs.PloutosBaseDomain,
                apiConfigs.RegisterPlayerUrl,
                apiConfigs.PloutosToken,
                playerProfile);

            var betTickets = new List<BetTicket>();
            await webClientService.PostAsync<List<BetTicket>>(
                apiConfigs.PloutosBaseDomain,
                apiConfigs.SendBetUrl,
                apiConfigs.PloutosToken,
                betTickets);            
        }    
    }
}
