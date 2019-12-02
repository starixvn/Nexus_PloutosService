namespace Ploutos.AutoJobs.Services
{
    using System.Threading.Tasks;

    public interface IEmailService
    {
        Task SendMail(string sitename, string subject, string body, string errorMessage, string url, string receiver);
    }
}
