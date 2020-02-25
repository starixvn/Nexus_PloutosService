using System;
using System.Collections.Generic;

namespace Ploutos.Common.Utils
{
    public interface IUtil
    {
        string BuildLogMessage(Exception exception);

        T ConvertJsonResponse<T>(string json);

        DateTime GetCurrentTimeAtGMTMinus4();

        string SerializeList<T>(IEnumerable<T> list);
    }
}
