using AutoFixture;
using FluentAssertions;
using NSubstitute;
using Ploutos.Common.Repository;
using Ploutos.Common.Utils;
using Ploutos.GlobalComJobs.Criteria;
using Ploutos.GlobalComJobs.Models;
using Ploutos.GlobalComJobs.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Ploutos.GlobalComJobs.Tests.Services
{
    public class DataServiceTest
    {
        private readonly Fixture fixture = new Fixture();

        [Fact]
        public async Task GetBetTickets_WhenCalled_GetBetTickets()
        {
            // Arrange
            var baseRepository = Substitute.For<IBaseRepository>();
            var util = Substitute.For<IUtil>();
            var service = new DataService(baseRepository, util);
            
            var betTickets = fixture.CreateMany<BetTicket>();
            var siteName = fixture.Create<string>();
            var batchSize = fixture.Create<int>();
            baseRepository.FetchData<BetTicket>(Arg.Any<GetBetTicketCriteria>()).Returns(betTickets);

            // Act
            var result = await service.GetBetTickets(siteName, batchSize);

            // Assert
            result.Should().BeEquivalentTo(betTickets);
        }

        [Fact]
        public async Task UpdateBetTickets_WhenCalled_UpdateBetTickets()
        {
            // Arrange
            var baseRepository = Substitute.For<IBaseRepository>();
            var util = Substitute.For<IUtil>();
            var service = new DataService(baseRepository, util);
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
            criteria.BetTicket.Should().Be(util.SerializeList<UpdateBetTicket>(betTickets));
        }
    }
}
