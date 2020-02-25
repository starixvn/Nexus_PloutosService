using Fanex.Data.Repository;
using Ploutos.Common.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ploutos.Common.Repository
{
    public interface IBaseRepository
    {
        Task<CommandResult> ExecuteNonQueryAsync(INonQueryCommand command);

        Task<IEnumerable<T>> FetchData<T>(ICriteria criteria);
    }
}
