namespace Ploutos.ServiceAPI.UnitTests.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoFixture;
    using Fanex.Data.Repository;
    using Fanex.Logging;
    using NSubstitute;
    using Ploutos.ServiceAPI.Criteria;
    using Ploutos.ServiceAPI.Models;
    using Ploutos.ServiceAPI.Services;
    using Xunit;

    public class PlayerProfileServiceTests
    {
        private readonly Fixture fixture = new Fixture();

        [Fact]
        public async Task InsertPlayerProfileAsync_PlayerProfileIsNull_ReturnEmpty()
        {
            // Arrange
            var logger = Substitute.For<ILogger>();
            var dynamicRepository = Substitute.For<IDynamicRepository>();
            var service = new PlayerProfileService(logger, dynamicRepository);

            // Act
            var actual = await service.InsertPlayerProfileAsync(null);

            // Assert
            Assert.Empty(actual);
        }

        [Fact]
        public async Task InsertPlayerProfileAsync_PlayerProfileIsEmpty_ReturnEmpty()
        {
            // Arrange
            var logger = Substitute.For<ILogger>();
            var dynamicRepository = Substitute.For<IDynamicRepository>();
            var service = new PlayerProfileService(logger, dynamicRepository);

            // Act
            var actual = await service.InsertPlayerProfileAsync(new List<PlayerProfile>());

            // Assert
            Assert.Empty(actual);
        }

        [Theory]
        [InlineData(1001)]
        [InlineData(1002)]
        [InlineData(1003)]
        public async Task InsertPlayerProfileAsync_ErrorCodeIsSpecial_ExpectedWriteLog(short errorCode)
        {
            // Arrange
            var logger = Substitute.For<ILogger>();
            var dynamicRepository = Substitute.For<IDynamicRepository>();
            var service = new PlayerProfileService(logger, dynamicRepository);
            var playerProfiles = fixture.CreateMany<PlayerProfile>().ToList();
            var playerResponseResults = fixture.CreateMany<PlayerResponseResult>().ToList();

            InsertPlayerProfileCriteria criteria = null;
            dynamicRepository.FetchAsync<PlayerResponseResult>
                (Arg.Do<InsertPlayerProfileCriteria>(x => { criteria = x; criteria.Error = errorCode; })).Returns(playerResponseResults);

            // Act
            var actual = await service.InsertPlayerProfileAsync(playerProfiles);

            // Assert
            Assert.Equal(playerResponseResults, actual);
            await logger.Received().ErrorAsync(Arg.Any<string>());
        }

        [Fact]
        public async Task InsertPlayerProfileAsync_ErrorCodeIsNotSpecial_ReturnAsExpected()
        {
            // Arrange
            var logger = Substitute.For<ILogger>();
            var dynamicRepository = Substitute.For<IDynamicRepository>();
            var service = new PlayerProfileService(logger, dynamicRepository);
            var playerProfiles = fixture.CreateMany<PlayerProfile>().ToList();
            var playerResponseResults = fixture.CreateMany<PlayerResponseResult>().ToList();

            InsertPlayerProfileCriteria criteria = null;
            dynamicRepository.FetchAsync<PlayerResponseResult>(Arg.Do<InsertPlayerProfileCriteria>(x => criteria = x)).Returns(playerResponseResults);

            // Act
            var actual = await service.InsertPlayerProfileAsync(playerProfiles);

            // Assert
            Assert.Equal(playerProfiles.Count, criteria.PlayerProfile.Rows.Count);
            Assert.Equal(playerResponseResults, actual);
            await logger.DidNotReceive().ErrorAsync(Arg.Any<string>());
        }
    }
}