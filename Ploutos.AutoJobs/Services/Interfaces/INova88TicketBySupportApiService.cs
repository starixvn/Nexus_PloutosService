using Ploutos.Common.Models;
using System.Threading.Tasks;

namespace Ploutos.AutoJobs.Services.Interfaces
{
    public interface INova88TicketBySupportApiService
    {
        Task<PushResult> PushTickets();
    }
}
