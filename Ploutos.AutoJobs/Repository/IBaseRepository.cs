namespace Ploutos.AutoJobs.Repository
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fanex.Data.Repository;
    using Ploutos.AutoJobs.Models;

    public interface IBaseRepository
    {
        Task<CommandResult> ExecuteNonQueryAsync(INonQueryCommand command);

        Task<IEnumerable<T>> FetchData<T>(ICriteria criteria);
    }
}
