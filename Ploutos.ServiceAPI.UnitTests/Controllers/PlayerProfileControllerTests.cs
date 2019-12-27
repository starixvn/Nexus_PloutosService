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
            Assert.All(actual, x => { Assert.Equal("timtrinh", x.Username); Assert.Equal("Duplicate Username.", x.ErrorMessage); });
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