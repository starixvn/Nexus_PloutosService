using Fanex.Data.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Ploutos.AutoJobs.Criteria
{
    public class UpdatePlayerProfileCriteria : NonQueryCommand
    {
        public string SiteName { get; set; }
        public DataTable PlayerProfile { get; set; }
        public override string GetSettingKey() => "API_PlayerProfile_Update";        
        public override bool IsValid() => true;
    }
}
