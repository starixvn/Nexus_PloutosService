namespace Ploutos.WebAPI.Controllers
{
    using System;
    using System.Web.Http;
    using Newtonsoft.Json.Linq;
    using Ploutos.WebAPI.Filters;
    using Ploutos.WebAPI.Models;

    public class AuthController : ApiController
    {
        [AllowAnonymous]
        [HttpPost]
        public string GetToken([FromBody]JObject data)
        {
            string username = data["username"].ToString();
            string password = data["password"].ToString();
            var token = string.Empty;
            if (IsValidCredential(username, password))
            {
                token = JwtManager.GenerateToken(username);
            }

            return token;
        }

        private static bool IsValidCredential(string username, string password)
        {
            return username.Equals(AppSettings.AppCredentialId, StringComparison.InvariantCultureIgnoreCase)
                && password.Equals(AppSettings.AppCredentialPassword, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}