namespace Ploutos.WebAPI.Services.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fanex.Data.Repository;
    using Ploutos.WebAPI.Models;

    public interface IBaseService
    {
        Task<IEnumerable<T>> GetDataAsync<T>(ICriteria criteria);

        Task<T> GetDataEntityAsync<T>(ICriteria criteria);

        Task<CommandResult> ExecuteNonQueryAsync(INonQueryCommand command);
    }
}