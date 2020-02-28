using System;
using System.Xml.Serialization;

namespace Ploutos.GlobalComJobs.Models
{
    [XmlType("r")]
    public class UpdateBetTicket
    {
        public UpdateBetTicket() { }

        public UpdateBetTicket(BetTicket betTicket)
        {
            this.Player_No = betTicket.player_no;
            this.Player_Name = betTicket.player_name;
            this.Player_Type = betTicket.player_type;
            this.Transaction_Date = betTicket.transaction_date;
            this.Transaction_Id = betTicket.transaction_id;
            this.Settlement_Date = betTicket.settlement_date;
            this.Brand = betTicket.brand;
            this.Brand_Url = betTicket.brand_url;
            this.Game_Code = betTicket.game_code;
            this.Game_Name = betTicket.game_name;
            this.Game_Type = betTicket.game_type;
            this.Currency_Code = betTicket.currency_code;
            this.Bet_Type = betTicket.bet_type;
            this.Bet_Amount = betTicket.bet_amount;
            this.Debit_Amount = betTicket.debit_amount;
            this.Credit_Amount = betTicket.credit_amount;
            this.Net_Amount = betTicket.net_amount;
        }

        [XmlAttribute("player_no")]
        public string Player_No { get; set; }

        [XmlAttribute("player_name")]
        public string Player_Name { get; set; }

        [XmlAttribute("player_type")]
        public string Player_Type { get; set; }

        [XmlAttribute("transaction_date")]
        public DateTime Transaction_Date { get; set; }

        [XmlAttribute("transaction_id")]
        public string Transaction_Id { get; set; }

        [XmlAttribute("settlement_date")]
        public DateTime Settlement_Date { get; set; }

        [XmlAttribute("brand")]
        public string Brand { get; set; }

        [XmlAttribute("brand_url")]
        public string Brand_Url { get; set; }

        [XmlAttribute("game_code")]
        public int Game_Code { get; set; }

        [XmlAttribute("game_name")]
        public string Game_Name { get; set; }

        [XmlAttribute("game_type")]
        public string Game_Type { get; set; }

        [XmlAttribute("currency_code")]
        public string Currency_Code { get; set; }

        [XmlAttribute("bet_type")]
        public string Bet_Type { get; set; }

        [XmlAttribute("bet_amount")]
        public double Bet_Amount { get; set; }

        [XmlAttribute("debit_amount")]
        public double Debit_Amount { get; set; }

        [XmlAttribute("credit_amount")]
        public double Credit_Amount { get; set; }

        [XmlAttribute("net_amount")]
        public double Net_Amount { get; set; }

        [XmlAttribute("is_error")]
        public bool Is_Error { get; set; }

        [XmlAttribute("response_result")]
        public string Response_Result { get; set; }        
    }
}
