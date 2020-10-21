namespace Ploutos.AutoJobs.Tests.Criteria
{
    using Ploutos.AutoJobs.Criteria;
    using Xunit;

    public class UpdateBetTicketCriteriaTests
    {
        [Fact]
        public void GetSettingKey_ReturnCorrectSettingKey()
        {
            // Arrange
            var criteria = new UpdateBetTicketCriteria();
            var expected = "Plo_API_BetTicket_Update";

            // Act
            var actual = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void IsValid_WhenCalled_ReturnExpectedSettingKey()
        {
            // Arrange
            var criteria = new UpdateBetTicketCriteria();

            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void Properties_WhenSet_GetExpectedValue()
        {
            // Arrange
            var criteria = new UpdateBetTicketCriteria
            {
                Error = 100,
                Message = "Error"
            };

            // Assert
            Assert.Equal("Error", criteria.Message);
            Assert.Equal(100, criteria.Error);
        }
    }
}
