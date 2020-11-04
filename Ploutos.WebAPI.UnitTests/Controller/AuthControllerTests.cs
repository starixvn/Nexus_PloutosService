namespace Ploutos.WebAPI.UnitTests.Controller
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Newtonsoft.Json.Linq;
    using Ploutos.WebAPI.Controllers;

    [TestClass]
    public class AuthControllerTests
    {
        [TestMethod]
        public void GetToken_IsInValidUsernamePassword_ReturnEmptyToken()
        {
            // Arrange
            var controller = new AuthController();

            dynamic jsonObject = new JObject();
            jsonObject.username = string.Empty;
            jsonObject.password = string.Empty;

            // Act
            var actual = controller.GetToken(jsonObject);

            // Assert
            Assert.IsTrue(string.IsNullOrEmpty(actual));
        }
    }
}
