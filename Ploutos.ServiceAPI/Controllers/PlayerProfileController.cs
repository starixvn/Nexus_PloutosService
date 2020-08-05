namespace Ploutos.ServiceAPI.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;
    using Ploutos.ServiceAPI.Enums;
    using Ploutos.ServiceAPI.Extensions;
    using Ploutos.ServiceAPI.Models;
    using Ploutos.ServiceAPI.Services;
    using Ploutos.ServiceAPI.ViewModels;

    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class PlayerProfileController : Controller
    {
        private readonly ApiConfigs apiConfigs;
        private readonly IPlayerProfileService playerProfileService;

        public PlayerProfileController(
            IOptionsMonitor<ApiConfigs> apiConfigs,
            IPlayerProfileService playerProfileService)
        {
            this.apiConfigs = apiConfigs.CurrentValue;
            this.playerProfileService = playerProfileService;
        }

        [HttpPost]
        public async Task<IActionResult> Post(IList<PlayerProfileViewModel> model)
        {
            var configuration = new MapperConfiguration(
                cfg => cfg.CreateMap<PlayerProfileViewModel, PlayerProfile>()
                    .ForMember(x => x.BrandCode, opt => opt.MapFrom(y => y.BrandCode.NullSafeTrim()))
                    .ForMember(x => x.CountryCode, opt => opt.MapFrom(y => y.CountryCode.NullSafeTrim()))
                    .ForMember(x => x.CurrencyCode, opt => opt.MapFrom(y => y.CurrencyCode.NullSafeTrim()))
                    .ForMember(x => x.EmailAddress, opt => opt.MapFrom(y => y.EmailAddress.NullSafeTrim()))
                    .ForMember(x => x.IPAddress, opt => opt.MapFrom(y => y.IPAddress.NullSafeTrim()))
                    .ForMember(x => x.LanguageCode, opt => opt.MapFrom(y => y.LanguageCode.NullSafeTrim()))
                    .ForMember(x => x.MobileCountryCode, opt => opt.MapFrom(y => y.MobileCountryCode.NullSafeTrim()))
                    .ForMember(x => x.MobileNumber, opt => opt.MapFrom(y => y.MobileNumber.NullSafeTrim()))
                    .ForMember(x => x.Password, opt => opt.MapFrom(y => y.Password.NullSafeTrim()))
                    .ForMember(x => x.Username, opt => opt.MapFrom(y => y.Username.NullSafeTrim()))
                    .ForMember(x => x.Username2, opt => opt.MapFrom(y => y.Username2.NullSafeTrim()))
            );
            var mapper = new Mapper(configuration);
            IList<PlayerProfile> playerProfiles = mapper.Map<IList<PlayerProfileViewModel>, IList<PlayerProfile>>(model);
            AutoIncreaseRowIndex(playerProfiles);

            Tuple<IList<PlayerProfile>, IList<PlayerResponseResult>> validateResult = ValidatePlayerProfiles(playerProfiles);
            var playerResponseResults = await playerProfileService.InsertPlayerProfileAsync(validateResult.Item1) as List<PlayerResponseResult>;

            playerResponseResults = CorrectErrorCode(playerResponseResults);

            playerResponseResults.AddRange(validateResult.Item2);
            playerResponseResults.AddRange(validateResult.Item1.Where(x => !playerResponseResults.Any(t => t.Username == x.Username))
                .Select(x => new PlayerResponseResult
                {
                    Username = x.Username,
                    Username2 = x.Username2,
                    ErrorCode = 0,
                    ErrorMessage = string.Empty
                }));

            return Json(playerResponseResults);
        }

        private static List<PlayerResponseResult> CorrectErrorCode(List<PlayerResponseResult> playerResponseResults)
        {
            foreach (var player in playerResponseResults)
            {
                if (player.ErrorCode == (int)ErrorCodes.DuplicateSyncParallelUsername)
                {
                    player.ErrorCode = (int)ErrorCodes.DuplicateUsername;
                    player.ErrorMessage = "The username (" + player.Username +") already exists.";
                }
            }

            return playerResponseResults;
        }

        private static void AutoIncreaseRowIndex(IList<PlayerProfile> playerProfiles)
        {
            int index = 1;
            foreach (var playerProfile in playerProfiles)
            {
                playerProfile.RowIndex = index++;
            }
        }

        private IList<PlayerResponseResult> CheckDuplicatePlayerProfiles(IList<PlayerProfile> playerProfiles)
        {
            var invalidProfiles = CheckDuplicateUsername(playerProfiles) as List<PlayerResponseResult>;
            invalidProfiles.AddRange(CheckDuplicateUsername2(playerProfiles));
            invalidProfiles.AddRange(CheckDuplicateEmailAddress(playerProfiles));
            return invalidProfiles;
        }

        private IList<PlayerResponseResult> CheckDuplicateUsername(IList<PlayerProfile> playerProfiles)
        {
            IEnumerable<string> duplicates = playerProfiles.Where(x => !string.IsNullOrWhiteSpace(x.Username)).GroupBy(x => x.Username)
                .Where(g => g.Count() > 1).Select(g => g.Key);
            return playerProfiles.Where(x => duplicates.Any(t => t == x.Username))
                .Select(x => new PlayerResponseResult
                {
                    RowIndex = x.RowIndex,
                    Username = x.Username,
                    Username2 = x.Username2,
                    ErrorCode = 102,
                    ErrorMessage = "Duplicate Username."
                }).ToList();
        }

        private IList<PlayerResponseResult> CheckDuplicateUsername2(IList<PlayerProfile> playerProfiles)
        {
            IEnumerable<string> duplicates = playerProfiles.Where(x => !string.IsNullOrWhiteSpace(x.Username2)).GroupBy(x => x.Username2)
                .Where(g => g.Count() > 1).Select(g => g.Key);
            return playerProfiles.Where(x => duplicates.Any(t => t == x.Username2))
                .Select(x => new PlayerResponseResult
                {
                    RowIndex = x.RowIndex,
                    Username = x.Username,
                    Username2 = x.Username2,
                    ErrorCode = 103,
                    ErrorMessage = "Duplicate Username2."
                }).ToList();
        }

        private IList<PlayerResponseResult> CheckDuplicateEmailAddress(IList<PlayerProfile> playerProfiles)
        {
            IEnumerable<string> duplicates = playerProfiles.Where(x => !string.IsNullOrWhiteSpace(x.EmailAddress)).GroupBy(x => x.EmailAddress)
                .Where(g => g.Count() > 1).Select(g => g.Key);
            return playerProfiles.Where(x => duplicates.Any(t => t == x.EmailAddress))
                .Select(x => new PlayerResponseResult
                {
                    RowIndex = x.RowIndex,
                    Username = x.Username,
                    Username2 = x.Username2,
                    ErrorCode = 104,
                    ErrorMessage = "Duplicate EmailAddress."
                }).ToList();
        }

        private Tuple<IList<PlayerProfile>, IList<PlayerResponseResult>> ValidatePlayerProfiles(IList<PlayerProfile> playerProfiles)
        {
            var validProfiles = new List<PlayerProfile>();
            var invalidProfiles = CheckDuplicatePlayerProfiles(playerProfiles);
            foreach (var playerProfile in playerProfiles)
            {
                PlayerResponseResult validateResult = ValidatePlayerProfile(playerProfile);
                if (!validateResult.IsSucessfull)
                {
                    invalidProfiles.Add(validateResult);
                }

                if (!string.IsNullOrWhiteSpace(playerProfile.BrandCode)
                    && !apiConfigs.BrandCodes.Any(t => t.Equals(playerProfile.BrandCode, StringComparison.CurrentCultureIgnoreCase)))
                {
                    invalidProfiles.Add(new PlayerResponseResult
                    {
                        RowIndex = playerProfile.RowIndex,
                        Username = playerProfile.Username,
                        Username2 = playerProfile.Username2,
                        ErrorCode = 101,
                        ErrorMessage = "BrandCode is invalid."
                    });
                }

                if (!invalidProfiles.Any(x => x.RowIndex == playerProfile.RowIndex))
                {
                    validProfiles.Add(playerProfile);
                }
            }

            invalidProfiles = invalidProfiles.GroupBy(x => new { x.RowIndex, x.Username }).Select(x => new PlayerResponseResult
            {
                Username = x.Key.Username,
                Username2 = x.FirstOrDefault()?.Username2,
                ErrorCode = 1,
                ErrorMessage = string.Join(" ", x.Select(t => t.ErrorMessage))
            }).ToList();

            return new Tuple<IList<PlayerProfile>, IList<PlayerResponseResult>>(validProfiles, invalidProfiles);
        }

        private PlayerResponseResult ValidatePlayerProfile(PlayerProfile playerProfile)
        {
            var playerResponse = new PlayerResponseResult();
            ModelState.Clear();
            TryValidateModel(playerProfile);

            if (!ModelState.IsValid)
            {
                List<string> failures = ModelState.Values.SelectMany(e => e.Errors).Select(gh => gh.ErrorMessage).ToList();

                playerResponse.RowIndex = playerProfile.RowIndex;
                playerResponse.Username = playerProfile.Username;
                playerResponse.Username2 = playerProfile.Username2;
                playerResponse.ErrorCode = 1;
                playerResponse.ErrorMessage = string.Join(" ", failures);
            }

            return playerResponse;
        }
    }
}