namespace Ploutos.ServiceAPI.Services
{
    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;
    using Ploutos.ServiceAPI.Criteria;
    using Ploutos.ServiceAPI.Models;
    using Ploutos.ServiceAPI.Repositories;

    public interface IPlayerProfileService
    {
        Task<CommandResult> InsertPlayerProfileAsync(IEnumerable<PlayerProfile> playerProfiles);
    }

    public class PlayerProfileService : IPlayerProfileService
    {
        private readonly IBaseRepository baseRepository;

        public PlayerProfileService(IBaseRepository baseRepository)
        {
            this.baseRepository = baseRepository;
        }

        public async Task<CommandResult> InsertPlayerProfileAsync(IEnumerable<PlayerProfile> playerProfiles)
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
            datatable.Columns.Add("PushTime");
            datatable.Columns.Add("ResponseResult");
            datatable.Columns.Add("ResponseTime");
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
                    profile.PushTime,
                    profile.ResponseResult,
                    profile.ResponseTime,
                    profile.Username,
                    profile.Username2);
            }

            var criteria = new InsertPlayerProfileCriteria
            {
                PlayerProfile = datatable
            };

            return await baseRepository.ExecuteNonQueryAsync(criteria);
        }
    }
}