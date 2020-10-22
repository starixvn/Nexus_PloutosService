namespace Ploutos.GlobalComJobs.Tests.Criteria
{
    using Ploutos.GlobalComJobs.Criteria;
    using Xunit;

    public class UpdateBetTicketCriteriaTests
    {
        [Fact]
        public void GetSettingKey_ReturnCorrectSettingKey()
        {
            // Arrange
            var criteria = new UpdateBetTicketCriteria();
            var expected = "Athena_API_BetTicket_Update";

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
                Error = 1,
                Message = "error"
            };

            // Assert
            Assert.Equal(1, criteria.Error);
            Assert.Equal("error", criteria.Message);
        }
    }
}
