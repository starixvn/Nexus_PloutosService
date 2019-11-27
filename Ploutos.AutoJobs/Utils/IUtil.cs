using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ploutos.AutoJobs.Utils
{
    public interface IUtil
    {
        string BuildLogMessage(Exception exception);

        T ConvertJsonResponse<T>(string json);

        DateTime GetCurrentTimeAtGMTMinus4();
    }
}
