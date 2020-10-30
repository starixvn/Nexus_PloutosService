namespace Ploutos.WebAPI.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Ploutos.WebAPI.Models;
    using Ploutos.WebAPI.Services;
    using Ploutos.WebAPI.Services.Interfaces;

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