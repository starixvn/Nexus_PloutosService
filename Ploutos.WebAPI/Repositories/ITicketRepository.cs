using Ploutos.WebAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ploutos.WebAPI.Repositories
{
    public interface ITicketRepository
    {
        Task<CommandResult> InserBetTickets(List<BetTicket> betTickets);
        Task<List<string>> GetDuplicateTicketData(List<BetTicket> betTickets);
        Task<TicketSettingData> GetTicketSettings();
    }
}
