using System.Threading.Tasks;

namespace Ploutos.Common.Repository
{
    public interface IRedisRepository
    {
        Task Set(string key, string value);

        Task<string> Get(string key);
    }
}
