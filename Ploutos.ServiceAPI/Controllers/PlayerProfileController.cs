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
            var configuration = new MapperConfiguration(cfg => cfg.CreateMap<PlayerProfileViewModel, PlayerProfile>());
            var mapper = new Mapper(configuration);
            IList<PlayerProfile> playerProfiles = mapper.Map<IList<PlayerProfileViewModel>, IList<PlayerProfile>>(model);

            Tuple<IList<PlayerProfile>, IList<PlayerResponseResult>> validateResult = ValidatePlayerProfiles(playerProfiles);
            var playerResponseResults = await playerProfileService.InsertPlayerProfileAsync(validateResult.Item1) as List<PlayerResponseResult>;

            playerResponseResults.AddRange(validateResult.Item2);
            playerResponseResults.AddRange(validateResult.Item1.Where(x => !playerResponseResults.Any(t => t.Username == x.Username))
                .Select(x => new PlayerResponseResult
                {
                    Username = x.Username,
                    ErrorCode = 0,
                    ErrorMessage = string.Empty
                }));

            return Json(playerResponseResults);
        }

        private Tuple<IList<PlayerProfile>, IList<PlayerResponseResult>> CheckDuplicatePlayerProfiles(IList<PlayerProfile> playerProfiles)
        {
            var invalidProfiles = CheckDuplicateUsername(playerProfiles) as List<PlayerResponseResult>;
            invalidProfiles.AddRange(CheckDuplicateUsername2(playerProfiles));
            invalidProfiles.AddRange(CheckDuplicateEmailAddress(playerProfiles));
            invalidProfiles.AddRange(CheckBrandCode(playerProfiles));
            invalidProfiles = invalidProfiles.GroupBy(x => x.Username).Select(x => new PlayerResponseResult
            {
                Username = x.Key,
                ErrorCode = 1,
                ErrorMessage = string.Join(" ", x.Select(t => t.ErrorMessage))
            }).ToList();

            IList<PlayerProfile> validProfiles = playerProfiles.Where(x => !invalidProfiles.Any(t => t.Username == x.Username)).ToList();
            return new Tuple<IList<PlayerProfile>, IList<PlayerResponseResult>>(validProfiles, invalidProfiles);
        }

        private IList<PlayerResponseResult> CheckDuplicateUsername(IList<PlayerProfile> playerProfiles)
        {
            IEnumerable<string> duplicates = playerProfiles.Where(x => !string.IsNullOrWhiteSpace(x.Username)).GroupBy(x => x.Username)
                .Where(g => g.Count() > 1).Select(g => g.Key);
            return playerProfiles.Where(x => duplicates.Any(t => t == x.Username))
                .Select(x => new PlayerResponseResult
                {
                    Username = x.Username,
                    ErrorCode = 1,
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
                    Username = x.Username,
                    ErrorCode = 1,
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
                    Username = x.Username,
                    ErrorCode = 1,
                    ErrorMessage = "Duplicate EmailAddress."
                }).ToList();
        }

        private IList<PlayerResponseResult> CheckBrandCode(IList<PlayerProfile> playerProfiles)
        {
            return playerProfiles.Where(x => !apiConfigs.BrandCodes.Any(t => t.Equals(x.BrandCode, StringComparison.CurrentCultureIgnoreCase)))
                .Select(x => new PlayerResponseResult
                {
                    Username = x.Username,
                    ErrorCode = 1,
                    ErrorMessage = "BrandCode is invalid."
                }).ToList();
        }

        private Tuple<IList<PlayerProfile>, IList<PlayerResponseResult>> ValidatePlayerProfiles(IList<PlayerProfile> playerProfiles)
        {
            Tuple<IList<PlayerProfile>, IList<PlayerResponseResult>> duplicateResult = CheckDuplicatePlayerProfiles(playerProfiles);
            IList<PlayerProfile> validProfiles = new List<PlayerProfile>();
            List<PlayerResponseResult> invalidProfiles = duplicateResult.Item2 as List<PlayerResponseResult>;
            foreach (var playerProfile in duplicateResult.Item1)
            {
                PlayerResponseResult validateResult = ValidatePlayerProfile(playerProfile);
                if (!validateResult.IsSucessfull)
                {
                    invalidProfiles.Add(validateResult);
                }
                else
                {
                    validProfiles.Add(playerProfile);
                }
            }

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

                playerResponse.Username = playerProfile.Username;
                playerResponse.ErrorCode = 1;
                playerResponse.ErrorMessage = string.Join(" ", failures);
            }

            return playerResponse;
        }
    }
}