using System;

namespace Ploutos.GlobalComJobs.Models
{
    public class BetTicket : BetTicketBase
    {
        public DateTime transaction_date { get; set; }
        public DateTime settlement_date { get; set; }        
    }
}
