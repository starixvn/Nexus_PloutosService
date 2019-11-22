using Fanex.Data.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ploutos.AutoJobs.Criteria
{
    public class GetPlayerProfileCriteria : CriteriaBase
    {
        public string SiteName { get; set; }
        public int TotalRows { get; set; }
        public override string GetSettingKey() => "API_PlayerProfile_SelPush";
        public override bool IsValid() => true;
    }
}
