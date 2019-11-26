using System;
using System.Collections.Generic;
using System.Text;

namespace Ploutos.AutoJobs.Models
{
    public class ProxySettings
    {
        public bool IsUseProxy { get; set; }
        public string ProxyAddress { get; set; }
        public bool BypassOnLocal { get; set; }
    }
}
