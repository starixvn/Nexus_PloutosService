using Ploutos.Common.Repository;
using System.Threading.Tasks;

namespace Ploutos.Common.Services
{
    public class JobRunningChecker : IJobRunningChecker
    {
        private readonly IRedisRepository redisRepository;
        private const string NAME_SPACE = "RunningJobs";
        private const string TRUE = "true";
        private const string FALSE = "false";

        public JobRunningChecker(IRedisRepository redisRepository)
        {
            this.redisRepository = redisRepository;
        }

        public async Task<bool> IsRunning(string jobName)
        {
            var value = await redisRepository.Get(GetKey(jobName));
            return value == TRUE;
        }

        public async Task MarkRunning(string jobName)
        {
            await redisRepository.Set(GetKey(jobName), TRUE);
        }

        public async Task ClearRunning(string jobName)
        {
            await redisRepository.Set(GetKey(jobName), FALSE);
        }

        private static string GetKey(string jobName)
        {
            return string.Format("{0}:{1}", NAME_SPACE, jobName);
        }
    }
}
