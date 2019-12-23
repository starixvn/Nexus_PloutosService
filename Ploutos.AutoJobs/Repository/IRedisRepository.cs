namespace Ploutos.AutoJobs.Repository
{
    using System.Threading.Tasks;

    public interface IRedisRepository
    {
        Task Set(string key, string value);

        Task<string> Get(string key);
    }
}