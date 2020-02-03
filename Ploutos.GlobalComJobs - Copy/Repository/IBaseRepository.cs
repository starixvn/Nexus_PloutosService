using Fanex.Data.Repository;
using Ploutos.GlobalComJobs.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ploutos.GlobalComJobs.Repository
{
    public interface IBaseRepository
    {
        Task<CommandResult> ExecuteNonQueryAsync(INonQueryCommand command);

        Task<IEnumerable<T>> FetchData<T>(ICriteria criteria);
    }
}
