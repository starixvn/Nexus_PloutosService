namespace Ploutos.ServiceAPI.Models
{
    using Newtonsoft.Json;

    public class TokenAccess
    {
        [JsonProperty("access_token")]
        public string Token { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("issued")]
        public string Issued { get; set; }

        [JsonProperty("expires")]
        public string Expires { get; set; }
    }
}