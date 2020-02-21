using Ploutos.GlobalComJobs.Criteria;
using Ploutos.GlobalComJobs.Models;
using Ploutos.GlobalComJobs.Repository;
using Ploutos.GlobalComJobs.Services.Interfaces;
using Ploutos.GlobalComJobs.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Ploutos.GlobalComJobs.Services
{
    public class DataService : IDataService
    {
        private readonly IBaseRepository baseRepository;
        private readonly IUtil util;

        public DataService(IBaseRepository baseRepository, IUtil util)
        {
            this.baseRepository = baseRepository;
            this.util = util;
        }

        public async Task<List<BetTicket>> GetBetTickets(string siteName, int batchSize)
        {
            var criteria = new GetBetTicketCriteria
            {
                BrandCode = siteName,
                TotalRows = batchSize
            };
            var betTickets = await this.baseRepository.FetchData<BetTicket>(criteria);

            return betTickets.ToList();
        }

        public Task<CommandResult> UpdateBetTickets(string siteName, DateTime pushTime, DateTime responseTime,
            string responseResult, List<UpdateBetTicket> betTickets)
        {
            var criteria = new UpdateBetTicketCriteria
            {
                BrandCode = siteName,
                PushTime = pushTime,
                ResponseTime = responseTime,
                ResponseResult = responseResult,
                BetTicket = this.util.SerializeList<UpdateBetTicket>(betTickets)
            };

            return this.baseRepository.ExecuteNonQueryAsync(criteria);
        }
       
    }
}
