namespace Ploutos.Common.Repository
{
    using Microsoft.Extensions.Options;
    using Ploutos.Common.Models;
    using StackExchange.Redis;
    using System.Threading.Tasks;

    public class RedisRepository : IRedisRepository
    {
        private readonly IOptions<RedisConfig> redisConfig;
        private readonly ConnectionMultiplexer connectionMultiplexer;

        public RedisRepository(IOptions<RedisConfig> redisConfig)
        {
            this.redisConfig = redisConfig;
            connectionMultiplexer = ConnectionMultiplexer.Connect(this.redisConfig.Value.Host);
        }

        public async Task<string> Get(string key)
        {
            var db = connectionMultiplexer.GetDatabase();
            return await db.StringGetAsync(GetKey(key));
        }

        public Task Set(string key, string value)
        {
            var db = connectionMultiplexer.GetDatabase();
            return db.StringSetAsync(GetKey(key), value);
        }

        private string GetKey(string jobName)
        {
            return string.Format("{0}:{1}", this.redisConfig.Value.Namespace, jobName);
        }
    }
}
