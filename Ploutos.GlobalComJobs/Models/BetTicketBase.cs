using System;
using System.Collections.Generic;
using System.Text;

namespace Ploutos.GlobalComJobs.Models
{
    public class BetTicketBase
    {
        public string player_no { get; set; }
        public string player_name { get; set; }
        public string player_type { get; set; }
        public string transaction_id { get; set; }
        public string brand { get; set; }
        public string brand_url { get; set; }
        public int game_code { get; set; }
        public string game_name { get; set; }
        public string game_type { get; set; }
        public string currency_code { get; set; }
        public string bet_type { get; set; }
        public double bet_amount { get; set; }
        public double debit_amount { get; set; }
        public double credit_amount { get; set; }
        public double net_amount { get; set; }
    }
}
