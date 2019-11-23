namespace Ploutos.AutoJobs.Services
{
    using System.Threading.Tasks;

    public interface IJobService
    {
        Task SyncNova88();

        Task SyncIvorybet();

        Task SyncOpal88();
    }
}