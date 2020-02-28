using System;
using System.Collections.Generic;
using System.Text;

namespace Ploutos.GlobalComJobs.Models
{
    public class ApiParam
    {
        public string provider_code { get; set; }
        public string operator_code { get; set; }
        public string aggregator_code { get; set; }
        public List<BetTicketToPush> game_transactions { get; set; }
    }
}
