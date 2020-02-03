namespace Ploutos.GlobalComJobs.Models
{
    public class BrandApiConfigs
    {
        public string BrandName { get; set; }
        public string BaseDomain { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string OperatorCode { get; set; }
        public string ProviderCode { get; set; }
        public UrlApiConfig TicketConfigs { get; set; }
        public UrlApiConfig AuthConfigs { get; set; }
    }
}
