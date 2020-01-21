using System.Threading.Tasks;

namespace Ploutos.GlobalComJobs.Services.Interfaces
{
    public interface IJobRunningChecker
    {
        Task<bool> IsRunning(string jobName);
        Task MarkRunning(string jobName);
        Task ClearRunning(string jobName);
        Task ClearAllRunning();
    }
}
