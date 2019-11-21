using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Ploutos.AutoJobs.Criteria;
using Ploutos.AutoJobs.Models;

namespace Ploutos.AutoJobs.Services
{
    public class DataService : IDataService
    {
        public async Task<List<BetTicket>> GetBetTickets(int batchSize)
        {
            var betTickets = new List<BetTicket>
            {
                FakeTiket(1351),
                FakeTiket2(1351),
                //FakeTiket(1351),
                //FakeTiket(1351),
                //FakeTiket(1352)
            };

            return betTickets;
        }

        public async Task<List<PlayerProfile>> GetPlayerProfiles(int batchSize)
        {
            var playerProfiles = new List<PlayerProfile>();

            for(var i = 0; i < 2; i++){
                playerProfiles.Add(FakePlayer());
            }

            return playerProfiles;
        }

        public Task<CommandResult> InsertBetTicketLog(InsertBetTicketLogCriteria criteria)
        {
            throw new NotImplementedException();
        }

        public Task<CommandResult> InsertPlayerProfileLog(InsertPlayerProfileLogCriteria criteria)
        {
            throw new NotImplementedException();
        }

        private PlayerProfile FakePlayer()
        {
            return new PlayerProfile
            {
                Username = "opal_test0002",
                Password = "p@@wordstronger",
                ConfirmPassword = "p@@wordstronger",
                EmailAddress = "opal_test0002@opal_test246.com",
                IPAddress = "0.0.0.1",
                BrandCode = "Opal88",
                LanguageCode = "EN",
                CurrencyCode = "CNY",
                MobileCountryCodeNumber = 84,
                PlayerTypeCode = "Real"
            };
        }

        private BetTicket FakeTiket(int transactionNo )
        {
            return new BetTicket
            {
                GamePlatformCode = "SB",
                SessionId = "strongpassword",
                RoundId = "HCM",
                GameId = "SB01",
                GameType = 1,
                TransactionDate = DateTime.Parse("2019-01-01"),
                TransactionNo = transactionNo,
                PlayerCode = "OPAL0191511441",
                BetAmount = 100,
                WinAmount = 0,
                LossAmount = 100
            };
        }

        private BetTicket FakeTiket2(int transactionNo)
        {
            return new BetTicket
            {
                RoundId = "HCM",
                GameId = "SB01",
                GameType = 1,
                TransactionDate = DateTime.Parse("2019-01-01"),
                TransactionNo = transactionNo,
                PlayerCode = "OPAL0191511441",
                BetAmount = 100,
                WinAmount = 0,
                LossAmount = 100
            };
        }
    }
}
