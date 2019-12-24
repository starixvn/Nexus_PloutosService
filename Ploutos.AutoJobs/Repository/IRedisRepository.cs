using System.Threading.Tasks;

namespace Ploutos.AutoJobs.Repository
{
    public interface IRedisRepository
    {
        Task Set(string key, string value);
        Task<string> Get(string key);
    }
}
