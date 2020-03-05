using Ploutos.Common.Models;
using System.Threading.Tasks;

namespace Ploutos.AutoJobs.Services.Interfaces
{
    public interface IOpal88TicketService
    {
        Task<PushResult> PushTickets();
    }
}
