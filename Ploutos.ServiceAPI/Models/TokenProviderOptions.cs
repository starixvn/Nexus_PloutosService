namespace Ploutos.ServiceAPI.Models
{
    using System.Text;
    using Microsoft.IdentityModel.Tokens;

    public class TokenProviderOptions
    {
        public int Expiration { get; set; }
        public string SecretKey { get; set; }
        public SymmetricSecurityKey SigningKey => new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));
        public SigningCredentials SigningCredentials => new SigningCredentials(SigningKey, SecurityAlgorithms.HmacSha256);
    }
}