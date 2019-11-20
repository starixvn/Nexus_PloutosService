using Fanex.Logging;
using Fanex.Logging.Extensions;
using Fanex.Logging.Extensions.Database;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ploutos.AutoJobs.Services
{
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
