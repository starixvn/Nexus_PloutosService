using Fanex.Data.Repository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Ploutos.AutoJobs.Criteria
{
    public class UpdateBetTicketCriteria : NonQueryCommand
    {
        public string SiteName { get; set; }
        public DataTable BetTicket { get; set; }
        public int Error { get; set; }
        public string Message { get; set; }
        public override string GetSettingKey() => "API_BetTicket_Update";
        public override bool IsValid() => true;
    }
}
