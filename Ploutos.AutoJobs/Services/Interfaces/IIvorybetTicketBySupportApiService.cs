using Ploutos.Common.Models;
using System.Threading.Tasks;

namespace Ploutos.AutoJobs.Services.Interfaces
{
    public interface IIvorybetTicketBySupportApiService
    {
        Task<PushResult> PushTickets();
    }
}
