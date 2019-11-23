using Ploutos.AutoJobs.Criteria;
using Ploutos.AutoJobs.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Ploutos.AutoJobs.Services
{
    public interface IDataService
    {
        Task<List<PlayerProfile>> GetPlayerProfiles(string siteName, int batchSize);
        Task<CommandResult> UpdatePlayerProfiles(string siteName, List<UpdatePlayerProfile> playerProfiles);
        Task<List<BetTicket>> GetBetTickets(string siteName, int batchSize);
        Task<CommandResult> UpdateBetTickets(string siteName, List<UpdateBetTicket> betTickets);
    }
}
