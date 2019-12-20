namespace Ploutos.AutoJobs.Models
{
    public class BrandApiConfigs
    {
        public string BrandName { get; set; }
        public string PloutosBaseDomain1 { get; set; }
        public string PloutosBaseDomain2 { get; set; }
        public string PloutosBaseDomain3 { get; set; }
        public string PloutosToken { get; set; }
        public UrlApiConfig PlayerConfigs { get; set; }
        public UrlApiConfig TicketConfigs { get; set; }
        public SupportApiConfigs SupportApiConfigs { get; set; }
        public UrlApiConfig SupportTicketConfigs { get; set; }
    }
}
