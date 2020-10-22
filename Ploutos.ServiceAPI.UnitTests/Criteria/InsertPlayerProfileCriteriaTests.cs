namespace Ploutos.ServiceAPI.UnitTests.Criteria
{
    using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
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

        [Fact]
        public void Properties_WhenSet_GetExpectedValue()
        {
            // Arrange
            var criteria = new InsertPlayerProfileCriteria
            {
                Message = "A"
            };

            // Assert
            Assert.Equal("A", criteria.Message);
        }
    }
}