namespace Ploutos.ServiceAPI.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Ploutos.ServiceAPI.Models;
    using Ploutos.ServiceAPI.Services;

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
        public async Task<IActionResult> Post([FromBody]IList<PlayerProfile> playerProfiles)
        {
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

        private static Tuple<IList<PlayerProfile>, IList<PlayerResponseResult>> ValidatePlayerProfiles(IList<PlayerProfile> playerProfiles)
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

        private static PlayerResponseResult ValidatePlayerProfile(PlayerProfile playerProfile)
        {
            PropertyInfo[] properties = typeof(PlayerProfile).GetProperties();
            List<string> failures = new List<string>();
            var playerResponse = new PlayerResponseResult();
            foreach (PropertyInfo property in properties)
            {
                var rules = Array.ConvertAll(
                    property.GetCustomAttributes(typeof(IValidationRule<PlayerProfile>), true),
                    item => (IValidationRule<PlayerProfile>)item);
                failures.AddRange(rules.Select(r => r.Validate(playerProfile)).Where(f => !string.IsNullOrWhiteSpace(f)));
            }

            if (failures.Any())
            {
                playerResponse.Username = playerProfile.Username;
                playerResponse.ErrorMessage = string.Join(";", failures);
            }

            return playerResponse;
        }
    }

    internal interface IValidationRule<in T>
    {
        string Validate(T input);
    }
}