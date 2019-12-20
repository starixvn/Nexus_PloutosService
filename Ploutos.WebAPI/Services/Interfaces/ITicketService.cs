using Ploutos.WebAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ploutos.WebAPI.Services.Interfaces
{
    public interface ITicketService
    {
        Task<TicketResponse> InsertTickets(List<BetTicket> betTickets);
    }
}