using Fanex.Data.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Ploutos.AutoJobs.Criteria
{
    public class UpdateBetTicketCriteria : CriteriaBase
    {
        public DataTable BetTickets { get; set; }
        public override string GetSettingKey() => "API_BetTicket_Update";
        public override bool IsValid() => true;
    }
}
