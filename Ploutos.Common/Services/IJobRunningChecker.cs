using System.Threading.Tasks;

namespace Ploutos.Common.Services
{
    public interface IJobRunningChecker
    {
        Task<bool> IsRunning(string jobName);
        Task MarkRunning(string jobName);
        Task ClearRunning(string jobName);
    }
}
