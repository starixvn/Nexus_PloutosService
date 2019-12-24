using System;

namespace Ploutos.WebAPI.Models
{
    public class BetTicket
    {
        public string GamePlatformCode { get; set; }
        public string GameId { get; set; }
        public int GameType { get; set; }
        public string TransactionDate { get; set; }
        public string TransactionNo { get; set; }
        public string PlayerCode { get; set; }
        public double BetAmount { get; set; }
        public double WinAmount { get; set; }
        public double LossAmount { get; set; }
        public string CurrencyCode { get; set; }
        public string CreatedById { get; set; }
        public string ModifiedById { get; set; }
        public string DateCreated { get; set; }
        public string DateModified { get; set; }
    }
}