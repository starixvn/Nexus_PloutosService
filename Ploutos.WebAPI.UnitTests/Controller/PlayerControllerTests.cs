namespace Ploutos.WebAPI.UnitTests.Controller
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using NSubstitute;
    using Ploutos.WebAPI.Controllers;
    using Ploutos.WebAPI.Services.Interfaces;
    using System.Threading.Tasks;

    [TestClass]
    public class PlayerControllerTests
    {
        [TestMethod]
        public async Task GetUserAccountNo_WhenUsernameInvalid_ReturnEmty()
        {
            // Arrange
            var playerService = Substitute.For<IPlayerService>();
            var controller = new PlayerController(playerService);
            string username = "starix";

            // Act
            var result = await controller.GetUserAccountNo(username);

            //Assert
            Assert.AreEqual("", result);
        }
    }
}
