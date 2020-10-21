namespace Ploutos.AutoJobs.Tests.Criteria
{
    using Ploutos.AutoJobs.Criteria;
    using System;
    using Xunit;

    public class GetBetTicketCriteriaTests
    {
        [Fact]
        public void GetSettingKey_ReturnCorrectSettingKey()
        {
            // Arrange
            var criteria = new GetBetTicketCriteria();
            var expected = "Plo_API_BetTicket_SelPush";

            // Act
            var actual = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("A", "2020-3-1", "2020-3-1")]
        [InlineData("A", "2020-3-1", "2020-3-2")]
        [InlineData(" ", "2020-3-2", "2020-3-1")]
        public void IsValid_ParamsAreInvalid_ReturnFalse(string brandCode, DateTime fromNewlyCutoffDate, DateTime toHistoricalDate)
        {
            // Arrange
            var criteria = new GetBetTicketCriteria
            {
                BrandCode = brandCode,
                FromNewlyCutoffDate = fromNewlyCutoffDate,
                ToHistoricalDate = toHistoricalDate
            };
            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.False(actual);
        }

        [Fact]
        public void IsValid_ParamsArevalid_ReturnTrue()
        {
            // Arrange
            var criteria = new GetBetTicketCriteria
            {
                BrandCode = "A",
                FromNewlyCutoffDate = DateTime.Now,
                ToHistoricalDate = DateTime.Now.AddDays(-1)
            };
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
                TotalRows = 100,
                TotalHistoricalRows = 99
            };

            // Assert
            Assert.Equal(99, criteria.TotalHistoricalRows);
            Assert.Equal(100, criteria.TotalRows);
        }
    }
}
