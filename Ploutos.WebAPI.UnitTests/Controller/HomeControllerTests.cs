namespace Ploutos.WebAPI.UnitTests.Controller
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Ploutos.WebAPI.Controllers;
    using System.Web.Mvc;

    [TestClass]
    public class HomeControllerTests
    {
        [TestMethod]
        public void Index_WhenCalled_ReturnAsExpected()
        {
            // Arrange
            var controller = new HomeController();

            // Act
            var actual = controller.Index() as ViewResult;

            // Assert
            Assert.IsTrue(string.IsNullOrEmpty(actual.ViewName));
        }
    }
}
