namespace Ploutos.GlobalComJobs.Services.Interfaces
{
    using System;
    using System.Threading.Tasks;
    using Fanex.Data.Repository;
    using Ploutos.Common.Models;

    public interface ILogServiceExtension
    {
        Task ErrorAsync(Exception exception);

        Task ErrorAsync(CommandResult commandResult, ICriteria criteria);

        void LogInformation(string message);
    }
}
