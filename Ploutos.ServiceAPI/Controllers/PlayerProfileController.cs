namespace Ploutos.ServiceAPI.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
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
        
        private readonly string InvalidMessage1 = "The {0} must be have characters long greater than zero and less than {1}.";
        private readonly string InvalidMessage2 = "The {0} must be have characters long greater than {1} and less than {2}.";
        private readonly int MaxLengthBrandCode = 20;
        private readonly int MaxLengthCurrencyCode = 10;
        private readonly int MaxLengthEmailAddress = 50;
        private readonly int MaxLengthIPAddress = 15;
        private readonly int MinLengthIPAddress = 7;
        private readonly int MaxLengthLanguageCode = 10;
        private readonly int MinLengthLanguageCode = 2;
        private readonly int MaxLengthPassword = 30;
        private readonly int MinLengthPassword = 6;
        private readonly int MaxLengthMobileCountryCode = 20;
        private readonly int MaxLengthMobileNumber = 16;
        private readonly int MinLengthMobileNumber = 7;
        private readonly int MaxLengthUsername = 30;

        public PlayerProfileController(IPlayerProfileService playerProfileService)
        {
            this.playerProfileService = playerProfileService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]IList<PlayerProfile> playerProfiles)
        {
            var inValidPlayers = GetInValidPlayerProfiles(playerProfiles);
            var validPlayers = playerProfiles.Where(t => !inValidPlayers.Any(x => x.Username.Equals(t.Username)));
            var playerErrorResponeResults = await playerProfileService.InsertPlayerProfileAsync(validPlayers.ToList());
            inValidPlayers = inValidPlayers.Union(playerErrorResponeResults).ToList();

            foreach (var player in validPlayers)
            {
                if (!inValidPlayers.Any(t => t.Username.Equals(player.Username)))
                {
                    playerErrorResponeResults.Add(
                        new PlayerResponeResult
                        {
                            Username = player.Username,
                            ErrorCode = 0,
                            ErrorMessage = string.Empty
                        });
                }
            }

            return Json(inValidPlayers);
        }

        private IList<PlayerResponeResult> GetInValidPlayerProfiles(IList<PlayerProfile> playerProfiles)
        {
            var players = new List<PlayerResponeResult>();
            foreach (var playerProfile in playerProfiles)
            {
                playerProfile.Username = playerProfile.Username ?? string.Empty;
                var playerValidateResult = ValidatePlayerProfile(playerProfile);
                if (!playerValidateResult.IsSucessfull)
                {
                    players.Add(playerValidateResult);
                }
            }

            return players;
        }

        private PlayerResponeResult ValidatePlayerProfile(PlayerProfile playerProfile)
        {
            var playerResponse = new PlayerResponeResult();
            if (string.IsNullOrWhiteSpace(playerProfile.Username) || playerProfile.Username.Length > MaxLengthUsername)
            {
                playerResponse.Username = playerProfile.Username;
                playerResponse.ErrorCode = 100;
                playerResponse.ErrorMessage = string.Format(InvalidMessage1, "UserName", MaxLengthUsername);
            }
            else if (string.IsNullOrWhiteSpace(playerProfile.Username2) || playerProfile.Username2.Length > MaxLengthUsername)
            {
                playerResponse.Username = playerProfile.Username;
                playerResponse.ErrorCode = 100;
                playerResponse.ErrorMessage = string.Format(InvalidMessage1, "UserName2", MaxLengthUsername);
            }
            else if (string.IsNullOrWhiteSpace(playerProfile.BrandCode) || playerProfile.BrandCode.Length > MaxLengthBrandCode)
            {
                playerResponse.Username = playerProfile.Username;
                playerResponse.ErrorCode = 100;
                playerResponse.ErrorMessage = string.Format(InvalidMessage1, "BrandCode", MaxLengthBrandCode);
            }
            else if (string.IsNullOrWhiteSpace(playerProfile.EmailAddress) || playerProfile.EmailAddress.Length > MaxLengthEmailAddress)
            {
                playerResponse.Username = playerProfile.Username;
                playerResponse.ErrorCode = 100;
                playerResponse.ErrorMessage = string.Format(InvalidMessage1, "EmailAddress", MaxLengthEmailAddress);
            }
            else if (string.IsNullOrWhiteSpace(playerProfile.CurrencyCode) || playerProfile.CurrencyCode.Length > MaxLengthCurrencyCode)
            {
                playerResponse.Username = playerProfile.Username;
                playerResponse.ErrorCode = 100;
                playerResponse.ErrorMessage = string.Format(InvalidMessage1, "CurrencyCode", MaxLengthCurrencyCode);
            }
            else if (string.IsNullOrWhiteSpace(playerProfile.MobileCountryCode) || playerProfile.MobileCountryCode.Length > MaxLengthMobileCountryCode)
            {
                playerResponse.Username = playerProfile.Username;
                playerResponse.ErrorCode = 100;
                playerResponse.ErrorMessage = string.Format(InvalidMessage1, "MobileCountryCode", MaxLengthMobileCountryCode);
            }
            else if (string.IsNullOrWhiteSpace(playerProfile.MobileNumber)
                || playerProfile.MobileNumber.Length > MaxLengthMobileNumber
                || playerProfile.MobileNumber.Length < MinLengthMobileNumber)
            {
                playerResponse.Username = playerProfile.Username;
                playerResponse.ErrorCode = 100;
                playerResponse.ErrorMessage = string.Format(InvalidMessage2, "MobileNumber", MinLengthMobileNumber, MaxLengthMobileNumber);
            }
            else if (string.IsNullOrWhiteSpace(playerProfile.IPAddress) 
                || playerProfile.IPAddress.Length < MinLengthIPAddress
                || playerProfile.IPAddress.Length > MaxLengthIPAddress)
            {
                playerResponse.Username = playerProfile.Username;
                playerResponse.ErrorCode = 100;
                playerResponse.ErrorMessage = string.Format(InvalidMessage2, "IPAddress", MinLengthIPAddress, MaxLengthIPAddress);
            }
            else if (string.IsNullOrWhiteSpace(playerProfile.LanguageCode)
                || playerProfile.LanguageCode.Length > MaxLengthLanguageCode
                || playerProfile.LanguageCode.Length < MinLengthLanguageCode)
            {
                playerResponse.Username = playerProfile.Username;
                playerResponse.ErrorCode = 100;
                playerResponse.ErrorMessage = string.Format(InvalidMessage2, "LanguageCode", MinLengthLanguageCode, MaxLengthLanguageCode);
            }
            else if (string.IsNullOrWhiteSpace(playerProfile.Password)
                || playerProfile.Password.Length > MaxLengthLanguageCode
                || playerProfile.Password.Length < MinLengthLanguageCode)
            {
                playerResponse.Username = playerProfile.Username;
                playerResponse.ErrorCode = 100;
                playerResponse.ErrorMessage = string.Format(InvalidMessage2, "Password", MinLengthPassword, MaxLengthPassword);
            }

            return playerResponse;
        }
    }
}