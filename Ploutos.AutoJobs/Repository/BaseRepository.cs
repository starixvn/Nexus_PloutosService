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
                using (IMultiResultReader multi = await db.QueryMultipleAsync(criteria).ConfigureAwait(false))
                {
                    return await multi.ReadAsync<T>();
                }
            }
        }

        private CommandResult GetCommandResult(Hashtable hash)
        {
            var errCode = hash["errCode"];
            var errMsg = hash["errMsg"];

            return new CommandResult
            {
                Code = (errCode is DBNull) ? 0 : Convert.ToInt32(errCode),
                Message = (errMsg is DBNull) ? string.Empty : Convert.ToString(errMsg)
            };
        }
    }
}
