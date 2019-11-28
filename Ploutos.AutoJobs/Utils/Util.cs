namespace Ploutos.AutoJobs.Utils
{
    using Fanex.Logging.Extensions;
    using Fanex.Logging.Extensions.Database;
    using Newtonsoft.Json;
    using System;

    public class Util : IUtil
    {
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
            return DateTime.UtcNow.AddHours(-4);
        }
    }
}
