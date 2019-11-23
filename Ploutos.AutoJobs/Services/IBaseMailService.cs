namespace Ploutos.AutoJobs.Services
{
    using System.Threading.Tasks;
    using Ploutos.AutoJobs.Models;

    public interface IBaseMailService
    {
        Task SendMailAsync(MailModel email);

        string ReadTextFile(string filePath);
    }
}
