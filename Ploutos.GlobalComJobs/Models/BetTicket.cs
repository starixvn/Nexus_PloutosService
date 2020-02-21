using System;

namespace Ploutos.GlobalComJobs.Models
{
    public class BetTicket
    {
        public string Player_No { get; set; }
        public string Player_Name { get; set; }
        public string Player_Type { get; set; }
        public DateTime Transaction_Date { get; set; }
        public string Transaction_Id { get; set; }
        public DateTime Settlement_Date { get; set; }
        public string Brand { get; set; }
        public string Brand_Url { get; set; }
        public int Game_Code { get; set; }
        public string Game_Name { get; set; }
        public string Game_Type { get; set; }
        public string Currency_Code { get; set; }
        public string Bet_Type { get; set; }
        public double Bet_Amount { get; set; }
        public double Debit_Amount { get; set; }
        public double Credit_Amount { get; set; }
        public double Net_Amount { get; set; }
    }
}
