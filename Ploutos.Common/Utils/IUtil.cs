namespace Ploutos.Common.Utils
{
    using System;
    using System.Collections.Generic;

    public interface IUtil
    {
        string BuildLogMessage(Exception exception);

        T ConvertJsonResponse<T>(string json);

        DateTime GetCurrentTimeAtGMTMinus4();

        List<List<T>> SplitToBatches<T>(List<T> data, int batchSize);
    }
}
