using Fanex.Data.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Ploutos.AutoJobs.Criteria
{
    public class InsertBetTicketLogCriteria : CriteriaBase
    {
        public DataTable BetRecords { get; set; }
        public override string GetSettingKey() => "API_BetRecord_InsertLog";
        public override bool IsValid() => true;
    }
}
