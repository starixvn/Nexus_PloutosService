namespace Ploutos.AutoJobs.Models
{
    public class BrandApiConfigs
    {
        public bool IsEnabled { get; set; }
        public string PloutosBaseDomain { get; set; }
        public string AffiliateBaseDomain { get; set; }
        public string RegisterPlayerUrl { get; set; }
        public string SendBetUrl { get; set; }
        public string PloutosToken { get; set; }
        public string AffiliateToken { get; set; }
        public int PlayerBatchSize { get; set; }
        public int TicketBatchSize { get; set; }
    }
}
