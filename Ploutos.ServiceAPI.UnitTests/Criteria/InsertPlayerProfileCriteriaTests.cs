namespace Ploutos.ServiceAPI.UnitTests.Criteria
{
    using Ploutos.ServiceAPI.Criteria;
    using Xunit;

    public class InsertPlayerProfileCriteriaTests
    {
        [Fact]
        public void GetSettingKey_WhenCalled_ReturnAsExpectedSettingKey()
        {
            // Arrange
            var criteria = new InsertPlayerProfileCriteria();

            // Act
            var actual = criteria.GetSettingKey();

            // Assert
            Assert.Equal("Plo_PlayerProfile_Insert", actual);
        }

        [Fact]
        public void IsValid_WhenCalled_ReturnTrue()
        {
            // Arrange
            var criteria = new InsertPlayerProfileCriteria();

            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.True(actual);
        }
    }
}