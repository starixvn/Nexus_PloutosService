namespace Ploutos.ServiceAPI.Services
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;
    using Fanex.Data.Repository;
    using Fanex.Logging;
    using Ploutos.ServiceAPI.Criteria;
    using Ploutos.ServiceAPI.Models;

    public interface IPlayerProfileService
    {
        Task<IList<PlayerResponseResult>> InsertPlayerProfileAsync(IList<PlayerProfile> playerProfiles);
    }

    public class PlayerProfileService : IPlayerProfileService
    {
        private readonly ILogger logger;
        private readonly IDynamicRepository dynamicRepository;

        public PlayerProfileService(
            ILogger logger,
            IDynamicRepository dynamicRepository)
        {
            this.logger = logger;
            this.dynamicRepository = dynamicRepository;
        }

        public async Task<IList<PlayerResponseResult>> InsertPlayerProfileAsync(IList<PlayerProfile> playerProfiles)
        {
            if (playerProfiles == null || !playerProfiles.Any())
            {
                return new List<PlayerResponseResult>();
            }

            var datatable = new DataTable();
            datatable.Columns.Add("BrandCode");
            datatable.Columns.Add("CountryCode");
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
                    profile.CountryCode,
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
            var playerResponseResults = await dynamicRepository.FetchAsync<PlayerResponseResult>(criteria);

            short[] errorCodes = new short[] { 1001, 1002, 1003 };
            if(errorCodes.Any(x => x == criteria.Error))
            {
                var message = string.Format("SP: {0}\n\rError: {1}\n\rMessage: {2}", "Plo_PlayerProfile_Insert", criteria.Error, criteria.Message);
                await logger.ErrorAsync(message);
            }

            return playerResponseResults.ToList();
        }
    }
}