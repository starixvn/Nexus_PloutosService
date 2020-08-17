namespace Ploutos.AutoJobs.Tests.Services
{
    using AutoFixture;
    using FluentAssertions;
    using NSubstitute;
    using Ploutos.AutoJobs.Criteria;
    using Ploutos.AutoJobs.Models;
    using Ploutos.AutoJobs.Services;
    using Ploutos.Common.Repository;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;

    public class DataServiceTest
    {
        private readonly Fixture fixture = new Fixture();

        [Fact]
        public async Task GetBetTickets_WhenCalled_GetBetTickets()
        {
            // Arrange
            var baseRepository = Substitute.For<IBaseRepository>();
            var service = new DataService(baseRepository);
            var betTickets = fixture.CreateMany<BetTicket>();
            var siteName = fixture.Create<string>();
            var batchSize = fixture.Create<int>();
            baseRepository.FetchData<BetTicket>(Arg.Any<GetBetTicketCriteria>()).Returns(betTickets);

            var criteria = new GetBetTicketCriteria
            {
                BrandCode = "A",
                FromNewlyCutoffDate = DateTime.Now,
                ToHistoricalDate = DateTime.Now.AddDays(-1)
            };

            // Act
            var result = await service.GetBetTickets(criteria);

            // Assert
            result.Should().BeEquivalentTo(betTickets);
        }

        [Fact]
        public async Task GetPlayerProfiles_WhenCalled_GetPlayerProfiles()
        {
            // Arrange
            var baseRepository = Substitute.For<IBaseRepository>();
            var service = new DataService(baseRepository);
            var playerProfiles = fixture.CreateMany<PlayerProfile>();
            var siteName = fixture.Create<string>();
            var batchSize = fixture.Create<int>();
            baseRepository.FetchData<PlayerProfile>(Arg.Any<GetPlayerProfileCriteria>()).Returns(playerProfiles);

            // Act
            var result = await service.GetPlayerProfiles(siteName, batchSize);

            // Assert
            result.Should().BeEquivalentTo(playerProfiles);
        }

        [Fact]
        public async Task UpdateBetTickets_WhenCalled_UpdateBetTickets()
        {
            // Arrange
            var baseRepository = Substitute.For<IBaseRepository>();
            var service = new DataService(baseRepository);
            var siteName = fixture.Create<string>();
            var pushTime = fixture.Create<DateTime>();
            var responseTime = fixture.Create<DateTime>();
            var responseResult = fixture.Create<string>();
            var betTickets = fixture.CreateMany<UpdateBetTicket>().ToList();

            UpdateBetTicketCriteria criteria = null;
            await baseRepository.ExecuteNonQueryAsync(Arg.Do<UpdateBetTicketCriteria>(x => criteria = x));

            // Act
            await service.UpdateBetTickets(siteName, pushTime, responseTime, responseResult, betTickets);

            // Assert
            criteria.BrandCode.Should().Be(siteName);
            criteria.PushTime.Should().Be(pushTime);
            criteria.ResponseTime.Should().Be(responseTime);
            criteria.ResponseResult.Should().Be(responseResult);
            criteria.BetTicket.Rows.Count.Should().Be(betTickets.Count);
        }

        [Fact]
        public async Task UpdatePlayerProfiles_WhenCalled_UpdatePlayerProfiles()
        {
            // Arrange
            var baseRepository = Substitute.For<IBaseRepository>();
            var service = new DataService(baseRepository);
            var siteName = fixture.Create<string>();
            var playerProfiles = fixture.CreateMany<UpdatePlayerProfile>().ToList();
            
            UpdatePlayerProfileCriteria criteria = null;
            await baseRepository.ExecuteNonQueryAsync(Arg.Do<UpdatePlayerProfileCriteria>(x => criteria = x));

            // Act
            await service.UpdatePlayerProfiles(siteName, playerProfiles);

            // Assert
            criteria.BrandCode.Should().Be(siteName);
            criteria.PlayerProfile.Rows.Count.Should().Be(playerProfiles.Count);
        }

        [Fact]
        public void PreProcessTickets_WhenCalled_UpdateTickets()
        {
            // Arrange
            var baseRepository = Substitute.For<IBaseRepository>();
            var service = new DataService(baseRepository);
            var betTickets = fixture.CreateMany<BetTicket>().ToList();

            var expectedTranDate = betTickets[0].TransactionDate.AddHours(-4);
            var expectedSetDate = betTickets[0].SettlementDate.AddHours(-4);

            // Act
            var result = service.PreProcessTickets(betTickets);

            // Assert
            result[0].TransactionDate.Should().Be(expectedTranDate);
            result[0].SettlementDate.Should().Be(expectedSetDate);
        }
    }
}
