namespace Ploutos.WebAPI.UnitTests.Criteria.Ticket
{
    using Ploutos.WebAPI.Criteria.Ticket;
    using Xunit;

    public class GetDuplicateTicketDataCriteriaTests
    {
        [Fact]
        public void GetSettingKey_ReturnCorrectSettingKey()
        {
            // Arrange
            var criteria = new GetDuplicateTicketDataCriteria();
            var expected = "SupportApi_BetTicketsDupSel";

            // Act
            var actual = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void IsValid_WhenCalled_ReturnExpectedSettingKey()
        {
            // Arrange
            var criteria = new GetDuplicateTicketDataCriteria();

            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void Properties_WhenSet_GetExpectedValue()
        {
            // Arrange
            var criteria = new GetDuplicateTicketDataCriteria
            {
                TransactionNoList = "test"
            };

            // Assert
            Assert.Equal("test", criteria.TransactionNoList);
        }
    }
}
