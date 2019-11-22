using System;
using System.Collections.Generic;
using System.Text;

namespace Ploutos.AutoJobs.Models
{
    public class UpdateBetTicket
    {
        public UpdateBetTicket(BetTicket betTicket)
        {
            GamePlatformCode = betTicket.GamePlatformCode;
        }

        public string GamePlatformCode { get; set; }
        public string SessionId { get; set; }
        public string RoundId { get; set; }
        public string GameId { get; set; }
        public int GameType { get; set; }
        public DateTime TransactionDate { get; set; }
        public int TransactionNo { get; set; }
        public string PlayerCode { get; set; }
        public double BetAmount { get; set; }
        public double WinAmount { get; set; }
        public double LossAmount { get; set; }
        public string ResponeResult { get; set; }
        public int TryCount { get; set; }
        public string Error { get; set; }
    }
}
