using Fanex.Data.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ploutos.AutoJobs.Criteria
{
    public class GetBetTicketCriteria : CriteriaBase
    {
        public string SiteName { get; set; }
        public int TotalRows { get; set; }
        public override string GetSettingKey() => "API_BetTicket_SelPush";
        public override bool IsValid() => true;
    }
}
