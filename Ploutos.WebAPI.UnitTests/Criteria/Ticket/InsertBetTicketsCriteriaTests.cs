namespace Ploutos.WebAPI.UnitTests.Criteria.Ticket
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Ploutos.WebAPI.Criteria.Ticket;

    [TestClass]
    public class InsertBetTicketsCriteriaTests
    {
        [TestMethod]
        public void GetSettingKey_ReturnCorrectSettingKey()
        {
            // Arrange
            var criteria = new InsertBetTicketsCriteria();
            var expected = "SupportApi_BetTicketsIns";

            // Act
            var actual = criteria.GetSettingKey();

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void IsValid_WhenCalled_ReturnExpectedSettingKey()
        {
            // Arrange
            var criteria = new InsertBetTicketsCriteria();

            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void Properties_WhenSet_GetExpectedValue()
        {
            // Arrange
            var criteria = new InsertBetTicketsCriteria
            {
                InsertCommand = "test",
                Error = 1,
                Message = "error"
            };

            // Assert
            Assert.AreEqual("test", criteria.InsertCommand);
            Assert.AreEqual(1, criteria.Error);
            Assert.AreEqual("error", criteria.Message);
        }
    }
}
