namespace Ploutos.AutoJobs.Services
{
    using Ploutos.AutoJobs.Criteria;
    using Ploutos.AutoJobs.Models;
    using Ploutos.Common.Models;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IDataService
    {
        Task<List<PlayerProfile>> GetPlayerProfiles(string siteName, int batchSize);

        Task<CommandResult> UpdatePlayerProfiles(string siteName, List<UpdatePlayerProfile> playerProfiles);

        Task<List<BetTicket>> GetBetTickets(GetBetTicketCriteria criteria);

        Task<CommandResult> UpdateBetTickets(string siteName, DateTime pushTime, DateTime responseTime,
            string responseResult, List<UpdateBetTicket> betTickets);

        List<BetTicket> PreProcessTickets(List<BetTicket> betTickets);
    }
}