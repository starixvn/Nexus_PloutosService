namespace Ploutos.WebAPI.UnitTests.Criteria.Ticket
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Ploutos.WebAPI.Criteria.Ticket;

    [TestClass]
    public class GetTicketSettingCriteriaTests
    {
        [TestMethod]
        public void GetSettingKey_ReturnCorrectSettingKey()
        {
            // Arrange
            var criteria = new GetTicketSettingCriteria();
            var expected = "SupportApi_SettingsInfoSel";

            // Act
            var actual = criteria.GetSettingKey();

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void IsValid_WhenCalled_ReturnExpectedSettingKey()
        {
            // Arrange
            var criteria = new GetTicketSettingCriteria();

            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.IsTrue(actual);
        }
    }
}
