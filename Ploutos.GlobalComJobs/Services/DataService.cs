namespace Ploutos.GlobalComJobs.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Ploutos.Common.Const;
    using Ploutos.Common.Models;
    using Ploutos.Common.Repository;
    using Ploutos.GlobalComJobs.Criteria;
    using Ploutos.GlobalComJobs.Extensions;
    using Ploutos.GlobalComJobs.Models;
    using Ploutos.GlobalComJobs.Services.Interfaces;

    public class DataService : IDataService
    {
        private readonly IBaseRepository baseRepository;

        public DataService(IBaseRepository baseRepository)
        {
            this.baseRepository = baseRepository;
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

        public List<BetTicket> PreProcessTickets(List<BetTicket> betTickets)
        {
            return betTickets.Select(t =>
            {
                t.transaction_date = t.transaction_date.AddHours(Consts.GmtMinus4);
                t.settlement_date = t.settlement_date.AddHours(Consts.GmtMinus4);
                return t;
            }).ToList();
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
                BetTicket = XmlHelper.Serialize(betTickets)
            };

            return this.baseRepository.ExecuteNonQueryAsync(criteria);
        }
    }
}
