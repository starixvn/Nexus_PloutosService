using Fanex.Data.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ploutos.AutoJobs.Criteria
{
    public class InsertPlayerProfileLogCriteria : CriteriaBase
    {
        public int CustId { get; set; }
        public string PlayerName { get; set; }
        public string BrandCode { get; set; }
        public string CurrencyCode { get; set; }
        public string LanguageCode { get; set; }
        public string PlayerTypeCode { get; set; }
        public string EmailAddress { get; set; }
        public string IPAddress { get; set; }
        public string MobileCountryCodeNumber { get; set; }
        public DateTime PushedTime { get; set; }
        public string MobileNumber { get; set; }
        public string DeviceType { get; set;}
        public int TryCount { get; set; }
        public string ResponeResult { get; set; }
        public override string GetSettingKey() => "API_PlayerProfile_InsertLog";        
        public override bool IsValid() => true;
    }
}
