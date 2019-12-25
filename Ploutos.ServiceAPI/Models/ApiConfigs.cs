namespace Ploutos.ServiceAPI.Models
{
    public class ApiConfigs
    {
        public string BrandCode { get; set; }

        public string[] BrandCodes => !string.IsNullOrWhiteSpace(BrandCode) ? BrandCode.Split(",") : new string[] { };
    }
}