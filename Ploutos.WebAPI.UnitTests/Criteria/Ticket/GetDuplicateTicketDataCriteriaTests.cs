namespace Ploutos.WebAPI.UnitTests.Criteria.Ticket
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Ploutos.WebAPI.Criteria.Ticket;

    [TestClass]
    public class GetDuplicateTicketDataCriteriaTests
    {
        [TestMethod]
        public void GetSettingKey_ReturnCorrectSettingKey()
        {
            // Arrange
            var criteria = new GetDuplicateTicketDataCriteria();
            var expected = "SupportApi_BetTicketsDupSel";

            // Act
            var actual = criteria.GetSettingKey();

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void IsValid_WhenCalled_ReturnExpectedSettingKey()
        {
            // Arrange
            var criteria = new GetDuplicateTicketDataCriteria();

            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void Properties_WhenSet_GetExpectedValue()
        {
            // Arrange
            var criteria = new GetDuplicateTicketDataCriteria
            {
                TransactionNoList = "test"
            };

            // Assert
            Assert.AreEqual("test", criteria.TransactionNoList);
        }
    }
}
