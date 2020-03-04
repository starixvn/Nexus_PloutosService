using System.Threading.Tasks;

namespace Ploutos.GlobalComJobs.Services.Interfaces
{
    public interface IJobService
    {
        Task Sync12BTicket();
        Task SyncOpal88Ticket();
        Task SyncNova88Ticket();
        Task SyncIvorybetTicket();
    }
}
