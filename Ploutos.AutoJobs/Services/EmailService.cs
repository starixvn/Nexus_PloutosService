namespace Ploutos.AutoJobs.Services
{
    using System;
    using System.Threading.Tasks;
    using Fanex.Logging;

    public class EmailService : IEmailService
    {
        private readonly ILogger logger;

        public EmailService(ILogger logger)
        {
            this.logger = logger;
        }

        public async Task SendMail(string sitename, string subject, string body, string errorMessage, string url, string receiver)
        {
            throw new NotImplementedException();
        }
    }
}