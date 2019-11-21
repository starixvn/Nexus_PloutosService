using System;
using System.Collections.Generic;
using System.Text;

namespace Ploutos.AutoJobs.Models
{
    public class PlayerProfile
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public int MobileCountryCodeNumber { get; set; }
        public string MobileNumber { get; set; }
        public string EmailAddress { get; set; }
        public string IPAddress { get; set; }
        public string BrandCode { get; set; }
        public string LanguageCode { get; set; }
        public string CurrencyCode { get; set; }
        public string CallbackUrl { get; set; }
        public string SignupCode { get; set; }
        public bool IsVerifiedEmail { get; set; }
        public bool IsverifiedMobile { get; set; }
        public bool IsCreatedByCS { get; set; }
        public string PlayerTypeCode { get; set; }
        public string AffiliateCode { get; set; }
        public string Platform { get; set; }
        public string SrcUrl { get; set; }
        public string Countrycode { get; set; }        
    }
}
