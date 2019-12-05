namespace Ploutos.AutoJobs.Services.Interfaces
{
    using System.Threading.Tasks;

    public interface IJobRunningChecker
    {
        Task<bool> IsRunning(string jobName);
        Task MarkRunning(string jobName);
        Task ClearRunning(string jobName);
        Task ClearAllRunning();
    }
}
