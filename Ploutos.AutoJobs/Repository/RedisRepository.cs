using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System.Threading.Tasks;

namespace Ploutos.AutoJobs.Repository
{
    public class RedisRepository : IRedisRepository
    {
        private readonly ConnectionMultiplexer connectionMultiplexer;

        public RedisRepository(IConfiguration configuration)
        {
            var redisConfig = configuration.GetValue<string>("RedisConfig");
            connectionMultiplexer = ConnectionMultiplexer.Connect(redisConfig);
        }

        public async Task<string> Get(string key)
        {
            var db = connectionMultiplexer.GetDatabase();
            return await db.StringGetAsync(key);            
        }

        public Task Set(string key, string value)
        {
            var db = connectionMultiplexer.GetDatabase();
            return db.StringSetAsync(key, value);            
        }
    }
}
