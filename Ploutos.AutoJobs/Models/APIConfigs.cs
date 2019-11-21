using System;
using System.Collections.Generic;
using System.Text;

namespace Ploutos.AutoJobs.Models
{
    public class ApiConfigs
    {        
        public string PloutosBaseDomain { get; set; }
        public string AffiliateBaseDomain { get; set; }
        public string RegisterPlayerUrl { get; set; }
        public string SendBetUrl { get; set; }
        public string PloutosToken { get; set; }
        public string AffiliateToken { get; set; }
        public int BatchSize { get; set; }
    }
}
