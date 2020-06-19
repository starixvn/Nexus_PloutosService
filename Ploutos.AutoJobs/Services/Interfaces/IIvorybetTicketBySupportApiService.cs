namespace Ploutos.AutoJobs.Services.Interfaces
{
    using Ploutos.Common.Models;
    using System.Threading.Tasks;

    public interface IIvorybetTicketBySupportApiService
    {
        Task<PushResult> PushTickets();
    }
}
