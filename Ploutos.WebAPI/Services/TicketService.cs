using Ploutos.WebAPI.Models;
using Ploutos.WebAPI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Ploutos.WebAPI.Services
{
    public class TicketService : ITicketService
    {
        public Task<TicketResponse> InsertTickets(List<BetTicket> betTickets)
        {
            throw new NotImplementedException();
        }
    }
}