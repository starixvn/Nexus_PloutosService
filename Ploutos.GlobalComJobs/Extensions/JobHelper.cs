namespace Ploutos.GlobalComJobs.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Ploutos.GlobalComJobs.Models;

    public static class JobHelper
    {
        public static JobSetting GetJobConfig(IEnumerable<JobSetting> jobConfigs, string jobName)
        {
            return jobConfigs.FirstOrDefault(job => job.JobName.Equals(jobName, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
