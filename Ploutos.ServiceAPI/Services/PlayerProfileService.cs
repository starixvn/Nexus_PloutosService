namespace Ploutos.ServiceAPI.Services
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;
    using Fanex.Data.Repository;
    using Ploutos.ServiceAPI.Criteria;
    using Ploutos.ServiceAPI.Models;

    public interface IPlayerProfileService
    {
        Task<IList<PlayerResponseResult>> InsertPlayerProfileAsync(IList<PlayerProfile> playerProfiles);
    }

    public class PlayerProfileService : IPlayerProfileService
    {
        private readonly IDynamicRepository dynamicRepository;

        public PlayerProfileService(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public async Task<IList<PlayerResponseResult>> InsertPlayerProfileAsync(IList<PlayerProfile> playerProfiles)
        {
            var datatable = new DataTable();
            datatable.Columns.Add("BrandCode");
            datatable.Columns.Add("CurrencyCode");
            datatable.Columns.Add("EmailAddress");
            datatable.Columns.Add("IPAddress");
            datatable.Columns.Add("IsDeposit");
            datatable.Columns.Add("LanguageCode");
            datatable.Columns.Add("MobileCountryCode");
            datatable.Columns.Add("MobileNumber");
            datatable.Columns.Add("Password");
            datatable.Columns.Add("PlayerTypeCode");
            datatable.Columns.Add("Username");
            datatable.Columns.Add("Username2");

            foreach (var profile in playerProfiles)
            {
                datatable.Rows.Add(
                    profile.BrandCode,
                    profile.CurrencyCode,
                    profile.EmailAddress,
                    profile.IPAddress,
                    profile.IsDeposit,
                    profile.LanguageCode,
                    profile.MobileCountryCode,
                    profile.MobileNumber,
                    profile.Password,
                    profile.PlayerTypeCode,
                    profile.Username,
                    profile.Username2);
            }

            var criteria = new InsertPlayerProfileCriteria
            {
                PlayerProfile = datatable
            };

            var playerResponeResults = await dynamicRepository.FetchAsync<PlayerResponseResult>(criteria).ConfigureAwait(false);
            return playerResponeResults.ToList();
        }
    }
}