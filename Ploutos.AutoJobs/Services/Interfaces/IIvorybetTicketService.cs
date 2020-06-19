namespace Ploutos.AutoJobs.Services.Interfaces
{
    using Ploutos.Common.Models;
    using System.Threading.Tasks;

    public interface IIvorybetTicketService
    {
        Task<PushResult> PushTickets();
    }
}
