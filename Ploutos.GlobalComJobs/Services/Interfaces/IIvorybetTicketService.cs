using Ploutos.Common.Models;
using System.Threading.Tasks;

namespace Ploutos.GlobalComJobs.Services.Interfaces
{
    public interface IIvorybetTicketService
    {
        Task<PushResult> PushTickets();
    }
}
