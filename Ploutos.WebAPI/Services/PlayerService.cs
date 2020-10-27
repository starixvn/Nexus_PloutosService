namespace Ploutos.WebAPI.Services
{
    using System.Threading.Tasks;
    using Fanex.Data.Repository;
    using Ploutos.WebAPI.Criteria.Player;
    using Ploutos.WebAPI.Repositories;
    using Ploutos.WebAPI.Services.Interfaces;

    public class PlayerService : BaseService, IPlayerService
    {
        public PlayerService(IDynamicRepository dynamicRepository = null, IBaseRepository baseRepository = null) :
            base(dynamicRepository, baseRepository)
        {
        }

        public async Task<string> GetUserAccountNoByUsername(string username)
        {
            var criteria = new GetUserAccountNumberByUsernameCriteria
            {
                Username = username
            };

            return await GetDataEntityAsync<string>(criteria);
        }
    }
}