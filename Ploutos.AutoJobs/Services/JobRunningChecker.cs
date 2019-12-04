using Ploutos.AutoJobs.Repository;
using Ploutos.AutoJobs.Services.Interfaces;
using System.Threading.Tasks;

namespace Ploutos.AutoJobs.Services
{
    public class JobRunningChecker : IJobRunningChecker
    {
        private readonly IRedisRepository redisRepository;
        private const string NAME_SPACE = "PloutosService:RunningJobs";
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

        public Task MarkRunning(string jobName)
        {
            return redisRepository.Set(GetKey(jobName), TRUE);
        }

        public Task ClearRunning(string jobName)
        {
            return redisRepository.Set(GetKey(jobName), FALSE);
        }

        private static string GetKey(string jobName)
        {
            return string.Format("{0}:{1}", NAME_SPACE, jobName);
        }             
    }
}
