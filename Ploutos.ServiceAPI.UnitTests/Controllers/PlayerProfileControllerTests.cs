namespace Ploutos.ServiceAPI.UnitTests.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
    using Microsoft.Extensions.Options;
    using NSubstitute;
    using Ploutos.ServiceAPI.Controllers;
    using Ploutos.ServiceAPI.Models;
    using Ploutos.ServiceAPI.Services;
    using Ploutos.ServiceAPI.ViewModels;
    using Xunit;

    public class PlayerProfileControllerTests
    {
        private IPlayerProfileService playerProfileService;

        [Fact]
        public async Task Post_DuplicateUsername_ReturnAsExpected()
        {
            // Arrange
            var model = new List<PlayerProfileViewModel>
            {
                new PlayerProfileViewModel { Username = "timtrinh" },
                new PlayerProfileViewModel { Username = "timtrinh" }
            };
            var controller = GetPlayerProfileController();
            var objectValidator = Substitute.For<IObjectModelValidator>();
            objectValidator.When(o => o.Validate(Arg.Any<ActionContext>(),
                Arg.Any<ValidationStateDictionary>(),
                Arg.Any<string>(),
                Arg.Any<Object>()));
            controller.ObjectValidator = objectValidator;
            playerProfileService.InsertPlayerProfileAsync(Arg.Any<IList<PlayerProfile>>()).Returns(new List<PlayerResponseResult>());

            // Act
            var result = await controller.Post(model);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            var actual = Assert.IsAssignableFrom<IList<PlayerResponseResult>>(jsonResult.Value);
            Assert.Equal(model.Count, actual.Count);
            Assert.All(actual, x =>
            {
                Assert.Equal("timtrinh", x.Username);
                Assert.Equal("Duplicate Username.", x.ErrorMessage);
            });
        }

        [Fact]
        public async Task Post_DuplicateUsername2_ReturnAsExpected()
        {
            // Arrange
            var model = new List<PlayerProfileViewModel>
            {
                new PlayerProfileViewModel { Username = "timxxxtrinh", Username2 = "timtrinh" },
                new PlayerProfileViewModel { Username = "timxxxxtrinh", Username2 = "timtrinh" }
            };
            var controller = GetPlayerProfileController();
            var objectValidator = Substitute.For<IObjectModelValidator>();
            objectValidator.When(o => o.Validate(Arg.Any<ActionContext>(),
                Arg.Any<ValidationStateDictionary>(),
                Arg.Any<string>(),
                Arg.Any<Object>()));
            controller.ObjectValidator = objectValidator;
            playerProfileService.InsertPlayerProfileAsync(Arg.Any<IList<PlayerProfile>>()).Returns(new List<PlayerResponseResult>());

            // Act
            var result = await controller.Post(model);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            var actual = Assert.IsAssignableFrom<IList<PlayerResponseResult>>(jsonResult.Value);
            Assert.Equal(model.Count, actual.Count);
            Assert.Collection(actual,
                x =>
                {
                    Assert.Equal("timxxxtrinh", x.Username);
                    Assert.Equal("Duplicate Username2.", x.ErrorMessage);
                },
                x =>
                {
                    Assert.Equal("timxxxxtrinh", x.Username);
                    Assert.Equal("Duplicate Username2.", x.ErrorMessage);
                });
        }

        [Fact]
        public async Task Post_DuplicateEmailAddress_ReturnAsExpected()
        {
            // Arrange
            var model = new List<PlayerProfileViewModel>
            {
                new PlayerProfileViewModel { Username = "timxxxtrinh", EmailAddress = "tim.trinh@starixsoft.com" },
                new PlayerProfileViewModel { Username = "timxxxxtrinh", EmailAddress = "tim.trinh@starixsoft.com" }
            };
            var controller = GetPlayerProfileController();
            var objectValidator = Substitute.For<IObjectModelValidator>();
            objectValidator.When(o => o.Validate(Arg.Any<ActionContext>(),
                Arg.Any<ValidationStateDictionary>(),
                Arg.Any<string>(),
                Arg.Any<Object>()));
            controller.ObjectValidator = objectValidator;
            playerProfileService.InsertPlayerProfileAsync(Arg.Any<IList<PlayerProfile>>()).Returns(new List<PlayerResponseResult>());

            // Act
            var result = await controller.Post(model);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            var actual = Assert.IsAssignableFrom<IList<PlayerResponseResult>>(jsonResult.Value);
            Assert.Equal(model.Count, actual.Count);
            Assert.Collection(actual,
                x =>
                {
                    Assert.Equal("timxxxtrinh", x.Username);
                    Assert.Equal("Duplicate EmailAddress.", x.ErrorMessage);
                },
                x =>
                {
                    Assert.Equal("timxxxxtrinh", x.Username);
                    Assert.Equal("Duplicate EmailAddress.", x.ErrorMessage);
                });
        }

        [Fact]
        public async Task Post_BrandCodeIsInvalid_ReturnAsExpected()
        {
            // Arrange
            var model = new List<PlayerProfileViewModel>
            {
                new PlayerProfileViewModel { Username = "timxxxtrinh", BrandCode = "12a" },
                new PlayerProfileViewModel { Username = "timxxxxtrinh", EmailAddress = "12bet" }
            };
            var controller = GetPlayerProfileController();
            var objectValidator = Substitute.For<IObjectModelValidator>();
            objectValidator.When(o => o.Validate(Arg.Any<ActionContext>(),
                Arg.Any<ValidationStateDictionary>(),
                Arg.Any<string>(),
                Arg.Any<Object>()));
            controller.ObjectValidator = objectValidator;
            playerProfileService.InsertPlayerProfileAsync(Arg.Any<IList<PlayerProfile>>()).Returns(new List<PlayerResponseResult>());

            // Act
            var result = await controller.Post(model);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            var actual = Assert.IsAssignableFrom<IList<PlayerResponseResult>>(jsonResult.Value);
            Assert.Equal(model.Count, actual.Count);
            Assert.Collection(actual,
                x =>
                {
                    Assert.Equal("timxxxtrinh", x.Username);
                    Assert.Equal("BrandCode is invalid.", x.ErrorMessage);
                },
                x =>
                {
                    Assert.Equal("timxxxxtrinh", x.Username);
                    Assert.Empty(x.ErrorMessage);
                });
        }

        [Fact]
        public async Task Post_ValidateField_ReturnAsExpected()
        {
            // Arrange
            var model = new List<PlayerProfileViewModel>
            {
                new PlayerProfileViewModel()
            };
            var controller = GetPlayerProfileController();
            var objectValidator = Substitute.For<IObjectModelValidator>();
            objectValidator.When(o => o.Validate(Arg.Any<ActionContext>(),
                Arg.Any<ValidationStateDictionary>(),
                Arg.Any<string>(),
                Arg.Any<Object>())).Do(x =>
                {
                    controller.ModelState.AddModelError("Username", "Username is required.");
                    controller.ModelState.AddModelError("Username2", "Username2 is required.");
                    controller.ModelState.AddModelError("BrandCode", "BrandCode is required.");
                });
            controller.ObjectValidator = objectValidator;
            playerProfileService.InsertPlayerProfileAsync(Arg.Any<IList<PlayerProfile>>()).Returns(new List<PlayerResponseResult>());

            // Act
            var result = await controller.Post(model);

            // Assert
            var jsonResult = Assert.IsType<JsonResult>(result);
            var actual = Assert.IsAssignableFrom<IList<PlayerResponseResult>>(jsonResult.Value);
            Assert.Equal(model.Count, actual.Count);
            Assert.Collection(actual, x =>
                {
                    Assert.Null(x.Username);
                    Assert.Contains("Username is required.", x.ErrorMessage);
                    Assert.Contains("Username2 is required.", x.ErrorMessage);
                    Assert.Contains("BrandCode is required.", x.ErrorMessage);
                });
        }

        private PlayerProfileController GetPlayerProfileController()
        {
            var apiConfigs = new ApiConfigs
            {
                BrandCode = "12bet"
            };
            var configs = Substitute.For<IOptionsMonitor<ApiConfigs>>();
            configs.CurrentValue.Returns(apiConfigs);
            playerProfileService = Substitute.For<IPlayerProfileService>();

            return new PlayerProfileController(configs, playerProfileService);
        }
    }
}