using Newtonsoft.Json;
using Ploutos.WebAPI.Criteria.Ticket;
using Ploutos.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var criteria = new InsertBetTicketsCriteria
            {
                BetTickets = JsonConvert.SerializeObject(betTickets)
            };
            var result = await this.baseRepository.ExecuteNonQueryAsync(criteria);
            return result;
        }

        public async Task<List<string>> GetTicketData()
        {
            var criteria = new GetTicketDataCriteria();
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