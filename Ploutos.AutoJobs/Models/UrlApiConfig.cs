using System;
using System.Collections.Generic;
using System.Text;

namespace Ploutos.AutoJobs.Models
{
    public class UrlApiConfig
    {
        public bool IsEnabled { get; set; }
        public string Url { get; set; }
        public int QueryBatchSize { get; set; }
        public int SyncBatchSize { get; set; }
    }
}
