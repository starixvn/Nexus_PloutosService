namespace Ploutos.WebAPI.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fanex.Data.Repository;
    using Ploutos.WebAPI.Models;
    using Ploutos.WebAPI.Repositories;
    using Ploutos.WebAPI.Services.Interfaces;

    public class BaseService : IBaseService
    {
        private readonly IDynamicRepository dynamicRepository;
        private readonly IBaseRepository baseRepository;

        public BaseService()
            : this(null, null)
        {
        }

        public BaseService(
            IDynamicRepository dynamicRepository,
            IBaseRepository baseRepository)
        {
            this.dynamicRepository = dynamicRepository ?? new DynamicRepository();
            this.baseRepository = baseRepository ?? new BaseRepository();
        }

        public virtual async Task<IEnumerable<T>> GetDataAsync<T>(ICriteria criteria)
        {
            if (criteria != null && criteria.IsValid())
            {
                return await dynamicRepository.FetchAsync<T>(criteria).ConfigureAwait(false);
            }

            return new List<T>();
        }

        public virtual async Task<T> GetDataEntityAsync<T>(ICriteria criteria)
        {
            if (criteria != null && criteria.IsValid())
            {
                return await dynamicRepository.GetAsync<T>(criteria).ConfigureAwait(false);
            }

            return default(T);
        }

        public virtual async Task<CommandResult> ExecuteNonQueryAsync(INonQueryCommand command)
        {
            var commandResult = new CommandResult();

            if (command != null && command.IsValid())
            {
                commandResult = await baseRepository.ExecuteNonQueryAsync(command).ConfigureAwait(false);
            }

            return commandResult;
        }
    }
}