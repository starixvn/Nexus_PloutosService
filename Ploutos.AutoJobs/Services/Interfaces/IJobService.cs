namespace Ploutos.AutoJobs.Services
{
    using System.Threading.Tasks;

    public interface IJobService
    {
        Task SyncOpal88Player();
        Task SyncOpal88Ticket();
        Task SyncNova88Player();
        Task SyncNova88Ticket();
        Task SyncIvorybetPlayer();
        Task SyncIvorybetTicket();
        Task SyncOpal88TicketBySupportApi();
        Task SyncNova88TicketBySupportApi();
        Task SyncIvorybetTicketBySupportApi();
        Task Sync12BPlayer();
        Task Sync12BTicket();
        Task Sync12BTicketBySupportApi();
    }
}