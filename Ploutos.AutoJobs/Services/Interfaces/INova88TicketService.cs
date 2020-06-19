namespace Ploutos.AutoJobs.Services.Interfaces
{
    using Ploutos.Common.Models;
    using System.Threading.Tasks;

    public interface INova88TicketService
    {
        Task<PushResult> PushTickets();
    }
}
