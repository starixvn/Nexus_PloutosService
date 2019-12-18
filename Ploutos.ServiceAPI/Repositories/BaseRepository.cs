namespace Ploutos.ServiceAPI.Repositories
{
    using System;
    using System.Collections;
    using System.Threading.Tasks;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Ploutos.ServiceAPI.Models;

    public interface IBaseRepository
    {
        Task<CommandResult> ExecuteNonQueryAsync(INonQueryCommand command);
    }

    public class BaseRepository : IBaseRepository
    {
        public async Task<CommandResult> ExecuteNonQueryAsync(INonQueryCommand command)
        {
            using (var db = ObjectDbFactory.CreateInstance(command.GetSettingKey()))
            {
                await db.ExecuteNonQueryAsync(command);
                return GetCommandResult(db.GetOutputParams());
            }
        }

        private CommandResult GetCommandResult(Hashtable hash)
        {
            var errCode = hash["Error"];
            var errMsg = hash["Message"];

            return new CommandResult
            {
                Error = (errCode is DBNull) ? 0 : Convert.ToInt32(errCode),
                Message = (errMsg is DBNull) ? string.Empty : Convert.ToString(errMsg)
            };
        }
    }
}