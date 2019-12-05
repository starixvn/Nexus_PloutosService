namespace Ploutos.AutoJobs.Services
{
    using Ploutos.AutoJobs.Constant;
    using Ploutos.AutoJobs.Repository;
    using Ploutos.AutoJobs.Services.Interfaces;
    using System.Threading.Tasks;

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

        public async Task ClearAllRunning()
        {
            await redisRepository.Set(GetKey(Constants.SyncOpal88Player), FALSE);
            await redisRepository.Set(GetKey(Constants.SyncOpal88Ticket), FALSE);
            await redisRepository.Set(GetKey(Constants.SyncNova88Player), FALSE);
            await redisRepository.Set(GetKey(Constants.SyncNova88Ticket), FALSE);
            await redisRepository.Set(GetKey(Constants.SyncIvorybetPlayer), FALSE);
            await redisRepository.Set(GetKey(Constants.SyncIvorybetTicket), FALSE);
        }
    }
}
