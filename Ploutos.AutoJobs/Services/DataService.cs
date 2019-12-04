namespace Ploutos.AutoJobs.Services
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;
    using Ploutos.AutoJobs.Criteria;
    using Ploutos.AutoJobs.Models;
    using Ploutos.AutoJobs.Repository;

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
                BrandCode = siteName,
                TotalRows = batchSize
            };
            var betTickets = await this.baseRepository.FetchData<BetTicket>(criteria);

            return betTickets.ToList();
        }

        public async Task<List<PlayerProfile>> GetPlayerProfiles(string siteName, int batchSize)
        {
            var criteria = new GetPlayerProfileCriteria()
            {
                BrandCode = siteName,
                TotalRows = batchSize
            };
            var playerProfiles = await this.baseRepository.FetchData<PlayerProfile>(criteria);

            return playerProfiles.ToList();
        }

        public Task<CommandResult> UpdateBetTickets(string siteName, DateTime pushTime, DateTime responseTime,
            string responseResult, List<UpdateBetTicket> betTickets)
        {
            DataTable data = new DataTable();
            data.Columns.Add("BetAmount");
            data.Columns.Add("CurrencyCode");
            data.Columns.Add("GameId");
            data.Columns.Add("GamePlatformCode");
            data.Columns.Add("GameType");
            data.Columns.Add("IsError");
            data.Columns.Add("LossAmount");
            data.Columns.Add("PlayerCode");
            data.Columns.Add("ResponseResult");
            data.Columns.Add("TransactionDate");
            data.Columns.Add("TransactionNo");
            data.Columns.Add("WinAmount");
            foreach (var ticket in betTickets)
            {
                data.Rows.Add(ticket.BetAmount, ticket.CurrencyCode, ticket.GameId, ticket.GamePlatformCode,
                    ticket.GameType, ticket.IsError, ticket.LossAmount, ticket.PlayerCode, ticket.ResponseResult,
                    ticket.TransactionDate, ticket.TransactionNo, ticket.WinAmount);
            }

            var criteria = new UpdateBetTicketCriteria()
            {
                BrandCode = siteName,
                PushTime = pushTime,
                ResponseTime = responseTime,
                ResponseResult = responseResult,
                BetTicket = data
            };

            return this.baseRepository.ExecuteNonQueryAsync(criteria);
        }

        public Task<CommandResult> UpdatePlayerProfiles(string siteName, List<UpdatePlayerProfile> playerProfiles)
        {
            DataTable data = new DataTable();
            data.Columns.Add("BrandCode");
            data.Columns.Add("CurrencyCode");
            data.Columns.Add("CustId");
            data.Columns.Add("EmailAddress");
            data.Columns.Add("IPAddress");
            data.Columns.Add("IsError");
            data.Columns.Add("LanguageCode");
            data.Columns.Add("Password");
            data.Columns.Add("PlayerCode");
            data.Columns.Add("PlayerTypeCode");
            data.Columns.Add("PushTime");
            data.Columns.Add("ResponseResult");
            data.Columns.Add("ResponseTime");
            data.Columns.Add("Username");

            foreach (var profile in playerProfiles)
            {
                data.Rows.Add(
                    profile.BrandCode, profile.CurrencyCode, profile.CustId, profile.EmailAddress,
                    profile.IPAddress, profile.IsError, profile.LanguageCode, profile.Password,
                    profile.PlayerCode, profile.PlayerTypeCode,
                    profile.PushTime, profile.ResponseResult, profile.ResponseTime, profile.Username);
            }

            var criteria = new UpdatePlayerProfileCriteria()
            {
                BrandCode = siteName,
                PlayerProfile = data
            };

            return this.baseRepository.ExecuteNonQueryAsync(criteria);
        }
    }
}