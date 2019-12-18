namespace Ploutos.ServiceAPI.Provider
{
    using System;
    using System.Text;
    using Microsoft.IdentityModel.Tokens;

    public class TokenProviderOptions
    {
        public string Path { get; set; } = "/token";
        public TimeSpan Expiration { get; set; }
        public string SecretKey { get; set; }
        public SymmetricSecurityKey SigningKey => new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));
        public SigningCredentials SigningCredentials => new SigningCredentials(SigningKey, SecurityAlgorithms.HmacSha256);
    }
}