namespace Ploutos.ServiceAPI.UnitTests.Services
{
    using System.Linq;
    using System.Threading.Tasks;
    using AutoFixture;
    using Fanex.Data.Repository;
    using NSubstitute;
    using Ploutos.ServiceAPI.Criteria;
    using Ploutos.ServiceAPI.Models;
    using Ploutos.ServiceAPI.Services;
    using Xunit;

    public class PlayerProfileServiceTests
    {
        private readonly Fixture fixture = new Fixture();

        [Fact]
        public async Task InsertPlayerProfileAsync_WhenCalled_InsertPlayerProfiles()
        {
            // Arrange
            var dynamicRepository = Substitute.For<IDynamicRepository>();
            var service = new PlayerProfileService(dynamicRepository);
            var playerProfiles = fixture.CreateMany<PlayerProfile>().ToList();
            var playerResponseResults = fixture.CreateMany<PlayerResponseResult>().ToList();

            InsertPlayerProfileCriteria criteria = null;
            dynamicRepository.FetchAsync<PlayerResponseResult>(Arg.Do<InsertPlayerProfileCriteria>(x => criteria = x)).Returns(playerResponseResults);

            // Act
            var actual = await service.InsertPlayerProfileAsync(playerProfiles);

            // Assert
            Assert.Equal(playerProfiles.Count, criteria.PlayerProfile.Rows.Count);
            Assert.Equal(playerResponseResults, actual);
        }
    }
}