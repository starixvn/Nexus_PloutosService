namespace Ploutos.AutoJobs.Tests.Criteria
{
    using Ploutos.AutoJobs.Criteria;
    using Xunit;

    public class UpdatePlayerProfileCriteriaTests
    {
        [Fact]
        public void GetSettingKey_ReturnCorrectSettingKey()
        {
            // Arrange
            var criteria = new UpdatePlayerProfileCriteria();
            var expected = "Plo_API_PlayerProfile_Update";

            // Act
            var actual = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void IsValid_WhenCalled_ReturnExpectedSettingKey()
        {
            // Arrange
            var criteria = new UpdatePlayerProfileCriteria();

            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.True(actual);
        }
    }
}
