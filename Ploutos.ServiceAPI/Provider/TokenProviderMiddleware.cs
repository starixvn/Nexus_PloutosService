namespace Ploutos.ServiceAPI.Provider
{
    using System;
    using System.Collections.Generic;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;
    using Ploutos.ServiceAPI.Models;

    public class TokenProviderMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IdentityConfigs _identityConfigs;
        private readonly TokenProviderOptions _options;

        public TokenProviderMiddleware(
            RequestDelegate next,
            IOptionsMonitor<IdentityConfigs> identityConfigs,
            IOptionsMonitor<TokenProviderOptions> options)
        {
            _next = next;
            _identityConfigs = identityConfigs.CurrentValue;
            _options = options.CurrentValue;
        }

        public Task Invoke(HttpContext context)
        {
            if (!context.Request.Path.Equals(_options.Path, StringComparison.Ordinal))
            {
                return _next(context);
            }

            if (!context.Request.Method.Equals("POST") || !context.Request.HasFormContentType)
            {
                context.Response.StatusCode = 400;
                return context.Response.WriteAsync("Bad request.");
            }

            return GenerateToken(context);
        }

        private async Task GenerateToken(HttpContext context)
        {
            var username = context.Request.Form["username"];
            var password = context.Request.Form["password"];
            var identity = await GetIdentity(username, password);

            if (identity == null)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("Invalid username or password.");
                return;
            }

            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                claims: identity.Claims,
                notBefore: now,
                expires: now.Add(_options.Expiration),
                signingCredentials: _options.SigningCredentials);
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                expires_in = (int)_options.Expiration.TotalSeconds,
            };

            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented }));
        }

        private Task<ClaimsIdentity> GetIdentity(string username, string password)
        {
            if (!username.Equals(_identityConfigs.Username) || !password.Equals(_identityConfigs.Password))
            {
                return Task.FromResult<ClaimsIdentity>(null);
            }

            IList<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, _identityConfigs.Username, null, ClaimsIdentity.DefaultIssuer, "Provider")
            };

            return Task.FromResult(new ClaimsIdentity(claims, "Bearer"));
        }
    }
}