using Fanex.Data.Repository;
using Ploutos.AutoJobs.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ploutos.AutoJobs.Repository
{
    public interface IBaseRepository
    {
        Task<CommandResult> ExecuteNonQueryAsync(INonQueryCommand command);

        Task<IEnumerable<T>> FetchData<T>(ICriteria criteria);
    }
}
