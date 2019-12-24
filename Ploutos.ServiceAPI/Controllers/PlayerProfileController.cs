namespace Ploutos.ServiceAPI.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Ploutos.ServiceAPI.Models;
    using Ploutos.ServiceAPI.Services;
    using Ploutos.ServiceAPI.ViewModels;

    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class PlayerProfileController : Controller
    {
        private readonly IPlayerProfileService playerProfileService;

        public PlayerProfileController(IPlayerProfileService playerProfileService)
        {
            this.playerProfileService = playerProfileService;
        }

        [HttpPost]
        public async Task<IActionResult> Post(IList<PlayerProfileViewModel> model)
        {
            var configuration = new MapperConfiguration(cfg => cfg.CreateMap<PlayerProfileViewModel, PlayerProfile>());
            var mapper = new Mapper(configuration);
            IList<PlayerProfile> playerProfiles = mapper.Map<IList<PlayerProfileViewModel>, IList<PlayerProfile>>(model);

            var validateResult = ValidatePlayerProfiles(playerProfiles);
            IList<PlayerResponseResult> playerResponseResults = await playerProfileService.InsertPlayerProfileAsync(validateResult.Item1);
            List<PlayerResponseResult> invalidProfiles = validateResult.Item2 as List<PlayerResponseResult>;
            invalidProfiles.AddRange(playerResponseResults.GroupBy(x => x.Username).Select(x => new PlayerResponseResult
            {
                Username = x.Key,
                ErrorMessage = string.Join("; ", x.Select(t => t.ErrorMessage))
            }));

            return Json(invalidProfiles);
        }

        private Tuple<IList<PlayerProfile>, IList<PlayerResponseResult>> ValidatePlayerProfiles(IList<PlayerProfile> playerProfiles)
        {
            IList<PlayerProfile> validProfiles = new List<PlayerProfile>();
            var invalidProfiles = new List<PlayerResponseResult>();
            foreach (var playerProfile in playerProfiles)
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
            TryValidateModel(playerProfile);

            if (!ModelState.IsValid)
            {
                List<string> failures = ModelState.Values.SelectMany(e => e.Errors).Select(gh => gh.ErrorMessage).ToList();

                playerResponse.Username = playerProfile.Username;
                playerResponse.ErrorCode = 1;
                playerResponse.ErrorMessage = string.Join(";", failures);
            }

            return playerResponse;
        }
    }
}