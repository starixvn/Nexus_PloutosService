namespace Ploutos.AutoJobs.Tests.Criteria
{
    using Ploutos.AutoJobs.Criteria;
    using Xunit;

    public class GetPlayerProfileCriteriaTests
    {
        [Fact]
        public void GetSettingKey_ReturnCorrectSettingKey()
        {
            // Arrange
            var criteria = new GetPlayerProfileCriteria();
            var expected = "Plo_API_PlayerProfile_SelPush";

            // Act
            var actual = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void IsValid_WhenCalled_ReturnExpectedSettingKey()
        {
            // Arrange
            var criteria = new GetPlayerProfileCriteria();

            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void Properties_WhenSet_GetExpectedValue()
        {
            // Arrange
            var criteria = new GetPlayerProfileCriteria
            {
                BrandCode = "A",
                TotalRows = 100
            };

            // Assert
            Assert.Equal("A", criteria.BrandCode);
            Assert.Equal(100, criteria.TotalRows);
        }
    }
}
