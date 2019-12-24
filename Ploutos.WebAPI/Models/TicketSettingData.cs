using System.Collections.Generic;

namespace Ploutos.WebAPI.Models
{
    public class TicketSettingData
    {
        public List<string> GamePlatformCodeList { get; set; }
        public List<string> GamePlatformProductCodeList { get; set; }
        public List<string> UserAccountNoList { get; set; }
    }
}