using Ploutos.Common.Models;
using System.Threading.Tasks;

namespace Ploutos.AutoJobs.Services.Interfaces
{
    public interface IIvorybetPlayerService
    {
        Task<PushResult> PushPlayers();
    }
}
