namespace Ploutos.ServiceAPI.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
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
        public async Task<IActionResult> Post([FromBody]IEnumerable<PlayerProfile> playerProfiles)
        {
            CommandResult commandResult = await playerProfileService.InsertPlayerProfileAsync(playerProfiles);

            return commandResult.IsSuccessful ? StatusCode(StatusCodes.Status200OK) : StatusCode(StatusCodes.Status304NotModified);
        }
    }
}