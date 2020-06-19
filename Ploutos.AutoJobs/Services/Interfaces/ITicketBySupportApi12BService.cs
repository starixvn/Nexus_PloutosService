namespace Ploutos.AutoJobs.Services.Interfaces
{
    using Ploutos.Common.Models;
    using System.Threading.Tasks;

    public interface ITicketBySupportApi12BService
    {
        Task<PushResult> PushTickets();
    }
}
