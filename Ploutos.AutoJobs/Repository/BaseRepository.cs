namespace Ploutos.AutoJobs.Repository
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fanex.Data;
    using Fanex.Data.Repository;
    using Ploutos.AutoJobs.Models;

    public class BaseRepository : IBaseRepository
    {
        public async Task<CommandResult> ExecuteNonQueryAsync(INonQueryCommand command)
        {
            using (var db = ObjectDbFactory.CreateInstance(command.GetSettingKey()))
            {
                await db.ExecuteNonQueryAsync(command).ConfigureAwait(false);
                CommandResult commandResult = GetCommandResult(db.GetOutputParams());

                return commandResult;
            }
        }

        public async Task<IEnumerable<T>> FetchData<T>(ICriteria criteria)
        {
            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
            {
                return await db.QueryAsync<T>(criteria);
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
