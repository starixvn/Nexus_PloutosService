using Newtonsoft.Json;
using Ploutos.WebAPI.Criteria.Ticket;
using Ploutos.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Ploutos.WebAPI.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly IBaseRepository baseRepository;

        public TicketRepository() : this(null)
        {
        }

        public TicketRepository(IBaseRepository baseRepository)
        {
            this.baseRepository = baseRepository ?? new BaseRepository();
        }

        public async Task<CommandResult> InserBetTickets(List<BetTicket> betTickets)
        {
            var sb = new List<string>();
            const string insertCommand = @"INSERT INTO Betlogs(GameId, GamePlatformCode, GameType" +
                ", LossAmount, WinAmount, BetAmount, PlayerCode, TransactionDate, SettlementDate" + 
                ", TransactionNo, CreatedById, ModifiedById, DateCreated, DateModified) VALUES ";            
            foreach (var ticket in betTickets)
            {
                sb.Add(string.Format(@"('{0}','{1}',{2},{3},{4},{5},'{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}')",
                    ticket.GameId, ticket.GamePlatformCode, ticket.GameType,
                    ticket.LossAmount, ticket.WinAmount, ticket.BetAmount,
                    ticket.PlayerCode, 
                    ticket.TransactionDate, ticket.SettlementDate, ticket.TransactionNo, ticket.CreatedById,
                    ticket.ModifiedById, ticket.DateCreated, ticket.DateModified));                
            }            

            var criteria = new InsertBetTicketsCriteria
            {
                InsertCommand = insertCommand + String.Join(",", sb)
            };
            var result = await this.baseRepository.ExecuteNonQueryAsync(criteria);
            return result;
        }

        public async Task<List<string>> GetDuplicateTicketData(List<BetTicket> betTickets)
        {
            var criteria = new GetDuplicateTicketDataCriteria {
                TransactionNoList = String.Join(",", betTickets.Select(t => t.TransactionNo))
            };
            var result = await this.baseRepository.GetDataAsync<string>(criteria);
            return result.ToList();
        }

        public async Task<TicketSettingData> GetTicketSettings()
        {
            var criteria = new GetTicketSettingCriteria();
            var result = await this.baseRepository.FetchMultipleAsync<string, string, string>(criteria);

            return new TicketSettingData
            {
                GamePlatformCodeList = result.Item1.ToList(),
                GamePlatformProductCodeList = result.Item2.ToList(),
                UserAccountNoList = result.Item3.ToList()
            };
        }
    }
}