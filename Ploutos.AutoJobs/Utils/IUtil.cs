namespace Ploutos.AutoJobs.Utils
{
    using System;

    public interface IUtil
    {
        string BuildLogMessage(Exception exception);

        T ConvertJsonResponse<T>(string json);

        DateTime GetCurrentTimeAtGMTMinus4();
    }
}