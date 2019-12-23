namespace Ploutos.AutoJobs.Models
{
    public class UrlApiConfig
    {
        public bool IsEnabled { get; set; }
        public string Url { get; set; }
        public int QueryBatchSize { get; set; }
        public int SyncBatchSize { get; set; }
        public int SyncParallelSize { get; set; }
    }
}
