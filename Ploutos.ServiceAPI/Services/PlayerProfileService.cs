namespace Ploutos.ServiceAPI.Services
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;
    using Fanex.Data.Repository;
    using Ploutos.ServiceAPI.Criteria;
    using Ploutos.ServiceAPI.Models;
    using Ploutos.ServiceAPI.Repositories;
    using System.Linq;

    public interface IPlayerProfileService
    {
        Task<IList<PlayerResponeResult>> InsertPlayerProfileAsync(IList<PlayerProfile> playerProfiles);
    }

    public class PlayerProfileService : IPlayerProfileService
    {
        private readonly IDynamicRepository dynamicRepository;

        public PlayerProfileService(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public async Task<IList<PlayerResponeResult>> InsertPlayerProfileAsync(IList<PlayerProfile> playerProfiles)
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
                    profile.BrandCode ?? string.Empty,
                    profile.CurrencyCode ?? string.Empty,
                    profile.EmailAddress ?? string.Empty,
                    profile.IPAddress ?? string.Empty,
                    profile.IsDeposit,
                    profile.LanguageCode ?? string.Empty,
                    profile.MobileCountryCode ?? string.Empty,
                    profile.MobileNumber ?? string.Empty,
                    profile.Password ?? string.Empty,
                    profile.PlayerTypeCode,
                    profile.Username ?? string.Empty,
                    profile.Username2 ?? string.Empty);
            }

            var criteria = new InsertPlayerProfileCriteria
            {
                PlayerProfile = datatable
            };

            var playerResponeResults = await dynamicRepository.FetchAsync<PlayerResponeResult>(criteria).ConfigureAwait(false);
            return playerResponeResults.ToList();
        }
    }
}