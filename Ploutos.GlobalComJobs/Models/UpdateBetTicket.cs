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
            this.Player_No = betTicket.Player_No;
            this.Player_Name = betTicket.Player_Name;
            this.Player_Type = betTicket.Player_Type;
            this.Transaction_Date = betTicket.Transaction_Date;
            this.Transaction_Id = betTicket.Transaction_Id;
            this.Settlement_Date = betTicket.Settlement_Date;
            this.Brand = betTicket.Brand;
            this.Brand_Url = betTicket.Brand_Url;
            this.Game_Code = betTicket.Game_Code;
            this.Game_Name = betTicket.Game_Name;
            this.Game_Type = betTicket.Game_Type;
            this.Currency_Code = betTicket.Currency_Code;
            this.Bet_Type = betTicket.Bet_Type;
            this.Bet_Amount = betTicket.Bet_Amount;
            this.Debit_Amount = betTicket.Debit_Amount;
            this.Credit_Amount = betTicket.Credit_Amount;
            this.Net_Amount = betTicket.Net_Amount;
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
