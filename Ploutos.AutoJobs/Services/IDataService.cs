using Ploutos.AutoJobs.Criteria;
using Ploutos.AutoJobs.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ploutos.AutoJobs.Services
{
    public interface IDataService
    {
        Task<List<PlayerProfile>> GetPlayerProfiles(int batchSize);
        Task<CommandResult> InsertPlayerProfileLog(InsertPlayerProfileLogCriteria criteria);
        Task<List<BetTicket>> GetBetTickets(int batchSize);
        Task<CommandResult> InsertBetTicketLog(InsertBetTicketLogCriteria criteria);
    }
}
