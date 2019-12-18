namespace Ploutos.ServiceAPI.Models
{
    using System;

    public class PlayerProfile
    {
        public string BrandCode { get; set; }
        public string CurrencyCode { get; set; }
        public string EmailAddress { get; set; }
        public string IPAddress { get; set; }
        public bool IsDeposit { get; set; }
        public string LanguageCode { get; set; }
        public string MobileCountryCode { get; set; }
        public string MobileNumber { get; set; }
        public string Password { get; set; }
        public DateTime PushTime { get; set; }
        public string ResponseResult { get; set; }
        public DateTime ResponseTime { get; set; }
        public string Username { get; set; }
        public string Username2 { get; set; }
    }
}