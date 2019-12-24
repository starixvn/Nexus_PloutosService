using Ploutos.WebAPI.Filters;
using Ploutos.WebAPI.Models;
using Ploutos.WebAPI.Services;
using Ploutos.WebAPI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Ploutos.WebAPI.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Http;

    public class TicketController : ApiController
    {
        private readonly ITicketService ticketService;

        public TicketController()
           : this(null)
        {
        }

        public TicketController(ITicketService ticketService)
        {
            this.ticketService = ticketService ?? new TicketService();
        }

        [HttpPost]
        public async Task<TicketResponse> InsertTickets(List<BetTicket> betTickets)
        {
            var result = await ticketService.InsertTickets(betTickets);
            return result;
        }
    }
}