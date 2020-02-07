using Ploutos.GlobalComJobs.Criteria;
using Ploutos.GlobalComJobs.Models;
using Ploutos.GlobalComJobs.Repository;
using Ploutos.GlobalComJobs.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploutos.GlobalComJobs.Services
{
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

        public Task<CommandResult> UpdateBetTickets(string siteName, DateTime pushTime, DateTime responseTime,
            string responseResult, List<UpdateBetTicket> betTickets)
        {
            DataTable data = new DataTable();
            data.Columns.Add("BetAmount");
            data.Columns.Add("CurrencyCode");
            data.Columns.Add("GameId");
            data.Columns.Add("GamePlatformCode");
            data.Columns.Add("GameType");
            data.Columns.Add("IsError");
            data.Columns.Add("LossAmount");
            data.Columns.Add("PlayerCode");
            data.Columns.Add("ResponseResult");
            data.Columns.Add("TransactionDate");
            data.Columns.Add("TransactionNo");
            data.Columns.Add("WinAmount");
            foreach (var ticket in betTickets)
            {
                data.Rows.Add(ticket.BetAmount, ticket.CurrencyCode, ticket.GameId, ticket.GamePlatformCode,
                    ticket.GameType, ticket.IsError, ticket.LossAmount, ticket.PlayerCode, ticket.ResponseResult,
                    ticket.TransactionDate, ticket.TransactionNo, ticket.WinAmount);
            }

            var criteria = new UpdateBetTicketCriteria
            {
                BrandCode = siteName,
                PushTime = pushTime,
                ResponseTime = responseTime,
                ResponseResult = responseResult,
                BetTicket = data
            };

            return this.baseRepository.ExecuteNonQueryAsync(criteria);
        }
    }
}
