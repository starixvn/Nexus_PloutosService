using Ploutos.WebAPI.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Ploutos.WebAPI.Controllers
{
    public class TicketController : ApiController
    {
        [HttpPost]
        public async Task<string> InsertTickets()
        {
            return "test result";
        }
    }
}