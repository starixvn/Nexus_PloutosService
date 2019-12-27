namespace Ploutos.ServiceAPI.UnitTests.Controllers
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Ploutos.ServiceAPI.Controllers;
    using Xunit;

    public class HomeControllerTests
    {
        [Fact]
        public void Get_WhenCalled_ReturnAsExpected()
        {
            // Arrange
            var controller = new HomeController();

            // Act
            var actual = controller.Get() as OkObjectResult;

            // Assert
            Assert.Equal(StatusCodes.Status200OK, actual.StatusCode);
            Assert.Equal("ok", actual.Value);
        }
    }
}