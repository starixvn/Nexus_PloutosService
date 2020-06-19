namespace Ploutos.AutoJobs.Services.Interfaces
{
    using Ploutos.Common.Models;
    using System.Threading.Tasks;

    public interface INova88PlayerService
    {
        Task<PushResult> PushPlayers();
    }
}
