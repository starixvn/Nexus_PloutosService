namespace Ploutos.WebAPI.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Http;

    public class TicketController : ApiController
    {
        [HttpPost]
        public async Task<string> InsertTickets()
        {
            return "test result";
        }
    }
}