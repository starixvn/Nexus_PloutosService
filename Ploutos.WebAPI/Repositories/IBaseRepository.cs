namespace Ploutos.WebAPI.Repositories
{
    using System.Threading.Tasks;
    using Fanex.Data.Repository;
    using Ploutos.WebAPI.Models;

    public interface IBaseRepository
    {
        Task<CommandResult> ExecuteNonQueryAsync(INonQueryCommand command);
    }
}