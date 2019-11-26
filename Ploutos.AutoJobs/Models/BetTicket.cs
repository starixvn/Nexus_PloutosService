namespace Ploutos.AutoJobs.Models
{
    using System;

    public class BetTicket
    {
        public string GamePlatformCode { get; set; }
        public string GameId { get; set; }
        public int GameType { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionNo { get; set; }
        public string PlayerCode { get; set; }
        public double BetAmount { get; set; }
        public double WinAmount { get; set; }
        public double LossAmount { get; set; }
        public string CurrencyCode { get; set; }
    }
}
