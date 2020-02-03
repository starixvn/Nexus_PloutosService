using System;

namespace Ploutos.GlobalComJobs.Utils
{
    public interface IUtil
    {
        string BuildLogMessage(Exception exception);

        T ConvertJsonResponse<T>(string json);

        DateTime GetCurrentTimeAtGMTMinus4();
    }
}
