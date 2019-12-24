namespace Ploutos.ServiceAPI.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;
    using Ploutos.ServiceAPI.Models;

    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : Controller
    {
        private readonly IdentityConfigs _identityConfigs;
        private readonly TokenProviderOptions _options;

        public TokenController(
            IOptionsMonitor<IdentityConfigs> identityConfigs,
            IOptionsMonitor<TokenProviderOptions> options)
        {
            _identityConfigs = identityConfigs.CurrentValue;
            _options = options.CurrentValue;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Post([FromBody]LoginModel login)
        {
            if (login == null
                || !_identityConfigs.Username.Equals(login.Username)
                || !_identityConfigs.Password.Equals(login.Password))
            {
                return BadRequest("Invalid username or password.");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, _identityConfigs.Username, null, ClaimsIdentity.DefaultIssuer, "Provider")
            };
            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                claims: claims,
                notBefore: now,
                expires: now.AddMinutes(_options.Expiration),
                signingCredentials: _options.SigningCredentials);
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new TokenAccess
            {
                Token = encodedJwt,
                ExpiresIn = _options.Expiration,
                Issued = jwt.ValidFrom.ToString("ddd, dd MMM yyyy HH:mm:ss 'GMT'", CultureInfo.InvariantCulture),
                Expires = jwt.ValidTo.ToString("ddd, dd MMM yyyy HH:mm:ss 'GMT'", CultureInfo.InvariantCulture)
            };

            return Json(response);
        }
    }
}