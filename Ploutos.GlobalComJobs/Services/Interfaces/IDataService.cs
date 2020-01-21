using Ploutos.GlobalComJobs.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ploutos.GlobalComJobs.Services.Interfaces
{
    public interface IDataService
    {
        Task<List<BetTicket>> GetBetTickets(string siteName, int batchSize);

        Task<CommandResult> UpdateBetTickets(string siteName, DateTime pushTime, DateTime responseTime,
            string responseResult, List<UpdateBetTicket> betTickets);
    }
}
