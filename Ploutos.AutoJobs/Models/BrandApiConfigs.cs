namespace Ploutos.AutoJobs.Models
{
    public class BrandApiConfigs
    {
        public string BrandName { get; set; }
        public string PloutosBaseDomain { get; set; }
        public string PloutosToken { get; set; }
        public UrlApiConfig PlayerConfigs { get; set; }
        public UrlApiConfig TicketConfigs { get; set; }
    }
}
