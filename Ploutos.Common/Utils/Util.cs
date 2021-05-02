namespace Ploutos.Common.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Logging.Extensions;
    using Fanex.Logging.Extensions.Database;
    using Newtonsoft.Json;
    using Ploutos.Common.Const;

    public class Util : IUtil
    {
        public const string RootElementName = "root";

        public string BuildLogMessage(Exception exception)
        {
            return new LogMessage(exception.Message)
                        .AddSection("Database Info", DatabaseInfoGetters.GetDbExceptionInfo(exception))
                        .AddSection("Server Info", CommonInfoGetters.GetServerInfo())
                        .AddSection("Exception Info", CommonInfoGetters.GetExceptionInfo(exception))
                        .ToString();
        }

        public T ConvertJsonResponse<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public DateTime GetCurrentTimeAtGMTMinus4()
        {
            return DateTime.UtcNow.AddHours(Consts.GmtMinus4);
        }

        public List<List<T>> SplitToBatches<T>(List<T> data, int batchSize)
        {
            return data.Select((s, i) => new { Value = s, Index = i })
                    .GroupBy(x => x.Index / batchSize)
                    .Select(grp => grp.Select(x => x.Value).ToList())
                    .ToList();
        }
    }
}
