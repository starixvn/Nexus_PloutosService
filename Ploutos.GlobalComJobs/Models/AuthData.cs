using System;

namespace Ploutos.GlobalComJobs.Models
{
    public class AuthData
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public double expires_in { get; set; }
        public DateTime expires_time { get; set; }
    }
}
