namespace Ploutos.WebAPI.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Ploutos.WebAPI.Filters;
    using Ploutos.WebAPI.Services;
    using Ploutos.WebAPI.Services.Interfaces;

    [JwtAuthorize]
    public class PlayerController : ApiController
    {
        private readonly IPlayerService playerService;

        public PlayerController()
           : this(null)
        {
        }

        public PlayerController(IPlayerService playerService)
        {
            this.playerService = playerService ?? new PlayerService();
        }

        [HttpGet]
        public async Task<string> GetUserAccountNo(string username)
        {
            string result = string.Empty;
            try
            {
                result = await playerService.GetUserAccountNoByUsername(username);
                WriteLogToFile.Writelog(result);
            }
            catch (Exception ex)
            {
                WriteLogToFile.Writelog(ex.Message);
                return ex.Message;
            }
            return result;
        }

        [HttpGet]
        public string Test(string username)
        {
            string result = string.Empty;
            try
            {
                result = "Test";
                WriteLogToFile.Writelog(result);
            }
            catch (Exception ex)
            {
                WriteLogToFile.Writelog(ex.Message);
                return ex.Message;
            }
            return result;
        }
    }
}