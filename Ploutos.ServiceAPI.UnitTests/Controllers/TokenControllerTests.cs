namespace Ploutos.ServiceAPI.UnitTests.Controllers
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;
    using NSubstitute;
    using Ploutos.ServiceAPI.Controllers;
    using Ploutos.ServiceAPI.Models;
    using Xunit;

    public class TokenControllerTests
    {
        private readonly Credentials credentials = new Credentials
        {
            Username = "aenfJVK95Xu2Znh8",
            Password = "app2!@#$%"
        };

        private readonly TokenProviderOptions tokenProviderOptions = new TokenProviderOptions
        {
            SecretKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ",
            Expiration = 120
        };

        public static IEnumerable<object[]> Post_InvalidParam_TestCase()
        {
            yield return new object[] { string.Empty, string.Empty };
            yield return new object[] { "username", string.Empty };
            yield return new object[] { string.Empty, "password" };
            yield return new object[] { "username", "password" };
        }

        [Theory, MemberData(nameof(Post_InvalidParam_TestCase))]
        public void Post_InvalidParam_ReturnAsExpected(string username, string password)
        {
            // Arrange
            LoginModel login = new LoginModel
            {
                Username = username,
                Password = password
            };
            var controller = GetTokenController();

            // Act
            var actual = controller.Post(login) as BadRequestObjectResult;

            // Assert
            Assert.Equal(StatusCodes.Status400BadRequest, actual.StatusCode);
        }

        [Fact]
        public void Post_ValidParam_ReturnAsExpected()
        {
            // Arrange
            var login = new LoginModel
            {
                Username = credentials.Username,
                Password = credentials.Password
            };
            var controller = GetTokenController();

            // Act
            var result = controller.Post(login);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            var actual = Assert.IsAssignableFrom<TokenAccess>(jsonResult.Value);
            Assert.Equal(tokenProviderOptions.Expiration, actual.ExpiresIn);
        }

        private TokenController GetTokenController()
        {
            var configs = Substitute.For<IOptionsMonitor<Credentials>>();
            var tokenOptions = Substitute.For<IOptionsMonitor<TokenProviderOptions>>();
            configs.CurrentValue.Returns(credentials);
            tokenOptions.CurrentValue.Returns(tokenProviderOptions);

            return new TokenController(configs, tokenOptions);
        }
    }
}