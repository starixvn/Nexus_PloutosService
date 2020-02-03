namespace Ploutos.GlobalComJobs.Models
{
    public class UrlApiConfig
    {
        public bool IsEnabled { get; set; }
        public string Url { get; set; }
        public int QueryBatchSize { get; set; }
        public int SyncBatchSize { get; set; }
    }
}
