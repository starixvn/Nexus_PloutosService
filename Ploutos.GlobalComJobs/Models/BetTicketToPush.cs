namespace Ploutos.GlobalComJobs.Models
{
    public class BetTicketToPush
    {
        public BetTicketToPush(BetTicket t)
        {
            player_no = t.player_no;
            player_name = t.player_name;
            player_type = t.player_type;
            transaction_date = t.transaction_date.ToString("yyyy-MM-ddTHH:mm:ss.fff");
            transaction_id = t.transaction_id;
            settlement_date = t.settlement_date.ToString("yyyy-MM-ddTHH:mm:ss.fff");
            brand = t.brand;
            brand_url = t.brand_url;
            game_code = t.game_code;
            game_name = t.game_name;
            game_type = t.game_type;
            currency_code = t.currency_code;
            bet_type = t.bet_type;
            bet_amount = t.bet_amount;
            debit_amount = t.debit_amount;
            credit_amount = t.credit_amount;
            net_amount = t.net_amount;
        }

        public string player_no { get; set; }
        public string player_name { get; set; }
        public string player_type { get; set; }
        public string transaction_date { get; set; }
        public string transaction_id { get; set; }
        public string settlement_date { get; set; }
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
