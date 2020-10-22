namespace Ploutos.GlobalComJobs.Tests.Criteria
{
    using Ploutos.GlobalComJobs.Criteria;
    using Xunit;

    public class GetBetTicketCriteriaTests
    {
        [Fact]
        public void GetSettingKey_ReturnCorrectSettingKey()
        {
            // Arrange
            var criteria = new GetBetTicketCriteria();
            var expected = "Athena_API_BetTicket_SelPush";

            // Act
            var actual = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void IsValid_WhenCalled_ReturnExpectedSettingKey()
        {
            // Arrange
            var criteria = new GetBetTicketCriteria();

            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void Properties_WhenSet_GetExpectedValue()
        {
            // Arrange
            var criteria = new GetBetTicketCriteria
            {
                BrandCode = "A",
                TotalRows = 10
            };

            // Assert
            Assert.Equal("A", criteria.BrandCode);
            Assert.Equal(10, criteria.TotalRows);
        }
    }
}
