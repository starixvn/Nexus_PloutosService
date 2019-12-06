using AutoFixture;
using Fanex.Data;
using Fanex.Data.Repository;
using FluentAssertions;
using NSubstitute;
using Ploutos.AutoJobs.Criteria;
using Ploutos.AutoJobs.Models;
using Ploutos.AutoJobs.Repository;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Ploutos.AutoJobs.Tests.Repository
{
    public class BaseRepositoryTest
    {
        private readonly IObjectDb objectDb;
        private readonly Fixture fixture;

        public BaseRepositoryTest()
        {
            DbSettingProviderManager.StartTestingSession().Run();
            objectDb = Substitute.For<IObjectDb>();
            ObjectDbFactory.Mock((key, time) => objectDb);
            fixture = new Fixture();
        }

        [Fact]
        public async Task ExecuteNonQueryAsync_WhenCalled_ExecuteNonQueryAsync()
        {
            // Arrange
            var repository = new BaseRepository();
            var command = new UpdateBetTicketCriteria();


            // Act
            var result = await repository.ExecuteNonQueryAsync(command);

            // Assert
            await objectDb.Received().ExecuteNonQueryAsync(command);
        }

        [Fact]
        public async Task ExecuteNonQueryAsync_WhenNoError_ReturnEmpty()
        {
            // Arrange
            var repository = new BaseRepository();
            var command = new UpdateBetTicketCriteria();

            var hash = new Hashtable();
            hash.Add("Error", DBNull.Value);
            hash.Add("Message", DBNull.Value);
            objectDb.GetOutputParams().Returns(hash);

            // Act
            var result = await repository.ExecuteNonQueryAsync(command);

            // Assert
            result.Error.Should().Be(0);
            result.Message.Should().BeEmpty();
        }

        [Fact]
        public async Task ExecuteNonQueryAsync_WhenError_ReturnErrors()
        {
            // Arrange
            var repository = new BaseRepository();
            var command = new UpdateBetTicketCriteria();

            var hash = new Hashtable();
            var errorCode = 123;
            var errorMsg = "some error";
            hash.Add("Error", errorCode);
            hash.Add("Message", errorMsg);
            objectDb.GetOutputParams().Returns(hash);

            // Act
            var result = await repository.ExecuteNonQueryAsync(command);

            // Assert
            result.Error.Should().Be(errorCode);
            result.Message.Should().Be(errorMsg);
        }

        [Fact]
        public async Task FetchData_WhenCalled_QueryAsync()
        {
            // Arrange
            var repository = new BaseRepository();
            var data = fixture.CreateMany<BetTicket>();            
            var criteria = fixture.Create<GetPlayerProfileCriteria>();
            objectDb.QueryAsync<BetTicket>(criteria).Returns(data);

            // Act
            var result = await repository.FetchData<BetTicket>(criteria);

            // Assert
            result.Should().BeEquivalentTo(data);
        }
    }
}
