using System;

namespace Ploutos.GlobalComJobs.Models
{
    public class UpdateBetTicket
    {
        public UpdateBetTicket(BetTicket betTicket)
        {
            BetAmount = betTicket.BetAmount;
            CurrencyCode = betTicket.CurrencyCode;
            GameId = betTicket.GameId;
            GamePlatformCode = betTicket.GamePlatformCode;
            GameType = betTicket.GameType;
            LossAmount = betTicket.LossAmount;
            PlayerCode = betTicket.PlayerCode;
            TransactionDate = betTicket.TransactionDate;
            TransactionNo = betTicket.TransactionNo;
            WinAmount = betTicket.WinAmount;
        }

        public double BetAmount { get; set; }
        public string CurrencyCode { get; set; }
        public string GameId { get; set; }
        public string GamePlatformCode { get; set; }
        public int GameType { get; set; }
        public bool IsError { get; set; }
        public double LossAmount { get; set; }
        public string PlayerCode { get; set; }
        public string ResponseResult { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionNo { get; set; }
        public double WinAmount { get; set; }
    }
}
