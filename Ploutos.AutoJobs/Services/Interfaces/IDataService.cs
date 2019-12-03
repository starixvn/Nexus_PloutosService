namespace Ploutos.AutoJobs.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Ploutos.AutoJobs.Models;

    public interface IDataService
    {
        Task<List<PlayerProfile>> GetPlayerProfiles(string siteName, int batchSize);

        Task<CommandResult> UpdatePlayerProfiles(string siteName, List<UpdatePlayerProfile> playerProfiles);

        Task<List<BetTicket>> GetBetTickets(string siteName, int batchSize);

        Task<CommandResult> UpdateBetTickets(string siteName, DateTime pushTime, DateTime responseTime,
            string responseResult, List<UpdateBetTicket> betTickets);
    }
}