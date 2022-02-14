namespace Ploutos.GlobalComJobs.Services.Interfaces
{
    using Ploutos.Common.Models;
    using Ploutos.GlobalComJobs.Models;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    public interface IDataService
    {
        Task<List<BetTicket>> GetBetTickets(string siteName, int batchSize, bool isTest);

        Task<CommandResult> UpdateBetTickets(string siteName, DateTime pushTime, DateTime responseTime,
            string responseResult, List<UpdateBetTicket> betTickets);

        List<BetTicket> PreProcessTickets(List<BetTicket> betTickets);
    }
}
