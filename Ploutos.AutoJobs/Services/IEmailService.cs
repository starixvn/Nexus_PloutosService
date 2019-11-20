using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ploutos.AutoJobs.Services
{
    public interface IEmailService
    {
        Task SendMail(string sitename, string subject, string body, string errorMessage, string url, string receiver);
    }
}
