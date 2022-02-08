namespace Ploutos.GlobalComJobs.Services
{
    using System;
    using System.Threading.Tasks;
    using Fanex.Data.Repository;
    using Fanex.Logging.Extensions;
    using Fanex.Logging.Extensions.Database;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using Ploutos.Common.Models;
    using Ploutos.GlobalComJobs.Services.Interfaces;

    public class LogServiceExtension : ILogServiceExtension
    {
        private readonly ILogger filelogger;
        private readonly Fanex.Logging.ILogger logger;

        public LogServiceExtension(
            Fanex.Logging.ILogger logger,
            ILogger<LogServiceExtension> filelogger)
        {
            this.logger = logger;
            this.filelogger = filelogger;
        }

        public async Task ErrorAsync(Exception exception)
        {
            var logMessage = new LogMessage(exception.Message)
                       .AddSection("Database Info", DatabaseInfoGetters.GetDbExceptionInfo(exception))
                       .AddSection("Server Info", CommonInfoGetters.GetServerInfo())
                       .AddSection("Exception Info", CommonInfoGetters.GetExceptionInfo(exception))
                       .ToString();
            await logger.ErrorAsync(logMessage);
        }

        public async Task ErrorAsync(CommandResult commandResult, ICriteria criteria)
        {
            var logMessage = new LogMessage(string.Format("{0}: {1}", commandResult.Error, commandResult.Message))
                       .AddMessage(JsonConvert.SerializeObject(criteria, Formatting.Indented))
                       .ToString();
            await logger.ErrorAsync(logMessage);
        }

        public void LogInformation(string message)
        {
            filelogger.LogInformation(message);
        }
    }
}
