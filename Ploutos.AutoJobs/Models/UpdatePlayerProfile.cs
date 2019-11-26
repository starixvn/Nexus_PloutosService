using System;

namespace Ploutos.AutoJobs.Models
{
    public class UpdatePlayerProfile
    {
        public UpdatePlayerProfile(PlayerProfile playerProfile)
        {
            CustId = playerProfile.CustId;
            Username = playerProfile.Username;
            Password = playerProfile.Password;
            EmailAddress = playerProfile.EmailAddress;
            IPAddress = playerProfile.IPAddress;
            BrandCode = playerProfile.BrandCode;
            LanguageCode = playerProfile.LanguageCode;
            CurrencyCode = playerProfile.CurrencyCode;
            PlayerTypeCode = playerProfile.PlayerTypeCode;

        }

        public int CustId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string EmailAddress { get; set; }
        public string IPAddress { get; set; }
        public string BrandCode { get; set; }
        public string LanguageCode { get; set; }
        public string CurrencyCode { get; set; }
        public string PlayerTypeCode { get; set; }
        public string ResponseResult { get; set; }
        public bool IsError { get; set; }
        public string PlayerCode { get; set; }
        public DateTime PushTime { get; set; }
        public DateTime ResponseTime { get; set; }
    }
}
