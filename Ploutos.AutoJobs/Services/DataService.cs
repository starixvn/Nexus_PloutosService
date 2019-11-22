using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ploutos.AutoJobs.Criteria;
using Ploutos.AutoJobs.Models;
using Ploutos.AutoJobs.Repository;

namespace Ploutos.AutoJobs.Services
{
    public class DataService : IDataService
    {
        private readonly IBaseRepository baseRepository;

        public DataService(IBaseRepository baseRepository)
        {
            this.baseRepository = baseRepository;
        }

        public async Task<List<BetTicket>> GetBetTickets(string siteName, int batchSize)
        {
            var criteria = new GetBetTicketCriteria()
            {
                SiteName = siteName,
                TotalRows = batchSize
            };
            var betTickets = await this.baseRepository.FetchData<BetTicket>(criteria);

            return betTickets.ToList();
            
            var betTicketss = new List<BetTicket>
            {
                FakeTiket(1351),
                FakeTiket2(1351),
                FakeTiket(1351),
                FakeTiket2(1351)
                //FakeTiket(1351),
                //FakeTiket(1351),
                //FakeTiket(1352)
            };
            //return betTickets;
        }

        public async Task<List<PlayerProfile>> GetPlayerProfiles(string siteName, int batchSize)
        {
            var criteria = new GetPlayerProfileCriteria()
            {
                SiteName = siteName, 
                TotalRows = batchSize
            };
            var playerProfiles = await this.baseRepository.FetchData<PlayerProfile>(criteria);            

            return playerProfiles.ToList();
        }        

        public Task<CommandResult> UpdateBetTickets(List<UpdateBetTicket> betTickets)
        {
            DataTable data = new DataTable();
            var criteria = new UpdateBetTicketCriteria()
            {
                BetTickets = data
            };

            return null;
        }

        public Task<CommandResult> UpdatePlayerProfiles(List<UpdatePlayerProfile> playerProfiles)
        {
            DataTable data = new DataTable();
            var criteria = new UpdatePlayerProfileCriteria()
            {
                PlayerProfiles = data
            };

            return null;
        }

        private PlayerProfile FakePlayer()
        {
            return new PlayerProfile
            {
                Username = "opal_test0004",
                Password = "p@@wordstronger",
                ConfirmPassword = "p@@wordstronger",
                EmailAddress = "opal_test0004@opal_test246.com",
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
