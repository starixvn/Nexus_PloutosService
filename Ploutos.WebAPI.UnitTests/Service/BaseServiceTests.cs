namespace Ploutos.WebAPI.UnitTests.Service
{
    using Fanex.Data.Repository;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using NSubstitute;
    using Ploutos.WebAPI.Models;
    using Ploutos.WebAPI.Repositories;
    using Ploutos.WebAPI.Services;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [TestClass]
    public class BaseServiceTests
    {
        private IDynamicRepository dynamicRepository;
        private IBaseRepository baseRepository;

        [TestMethod]
        public async Task GetDataAsync_CriteriaIsNull_ReturnEmptyList()
        {
            // Arrange
            var baseService = GetBaseService();

            // Act
            var actual = await baseService.GetDataAsync<int>(null);

            // Assert
            Assert.AreEqual(actual.Count(), 0);
        }

        [TestMethod]
        public async Task GetDataAsync_CriteriaIsInvalid_ReturnEmptyList()
        {
            // Arrange
            var criteria = Substitute.For<ICriteria>();
            criteria.IsValid().Returns(false);
            var baseService = GetBaseService();

            // Act
            var actual = await baseService.GetDataAsync<int>(criteria);

            // Assert
            Assert.AreEqual(actual.Count(), 0);
        }

        [TestMethod]
        public async Task GetDataAsync_CriteriaIsValid_ReturnExpectedData()
        {
            // Arrange
            var expected = new List<int> { 1, 2 };
            var criteria = Substitute.For<ICriteria>();
            criteria.IsValid().Returns(true);
            var baseService = GetBaseService();
            dynamicRepository.FetchAsync<int>(criteria).Returns(expected);

            // Act
            var actual = await baseService.GetDataAsync<int>(criteria);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public async Task GetDataEntityAsync_CriteriaIsNull_ReturnDefaultValue()
        {
            // Arrange
            var baseService = GetBaseService();

            // Act
            var actual = await baseService.GetDataEntityAsync<int>(null);

            // Assert
            Assert.AreEqual(0, actual);
        }

        [TestMethod]
        public async Task GetDataEntityAsync_CriteriaIsInvalid_ReturnDefaultValue()
        {
            // Arrange
            var criteria = Substitute.For<ICriteria>();
            criteria.IsValid().Returns(false);
            var baseService = GetBaseService();

            // Act
            var actual = await baseService.GetDataEntityAsync<int>(criteria);

            // Assert
            Assert.AreEqual(0, actual);
        }

        [TestMethod]
        public async Task GetDataEntityAsync_CriteriaIsValid_ReturnExpectedData()
        {
            // Arrange
            var expected = 1;
            var criteria = Substitute.For<ICriteria>();
            criteria.IsValid().Returns(true);
            var baseService = GetBaseService();
            dynamicRepository.GetAsync<int>(criteria).Returns(expected);

            // Act
            var actual = await baseService.GetDataEntityAsync<int>(criteria);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public async Task ExecuteNonQueryAsync_CriteriaIsNull_ReturnDefaultCommandResult()
        {
            // Arrange
            var expected = new CommandResult();
            var baseService = GetBaseService();

            // Act
            var actual = await baseService.ExecuteNonQueryAsync(null);

            // Assert
            Assert.AreEqual(expected.Error, actual.Error);
            Assert.AreEqual(expected.IsSuccessful, actual.IsSuccessful);
            Assert.AreEqual(expected.Message, actual.Message);
        }

        [TestMethod]
        public async Task ExecuteNonQueryAsync_CriteriaIsInvalid_ReturnDefaultCommandResult()
        {
            // Arrange
            var expected = new CommandResult();
            var criteria = Substitute.For<INonQueryCommand>();
            criteria.IsValid().Returns(false);
            var baseService = GetBaseService();

            // Act
            var actual = await baseService.ExecuteNonQueryAsync(criteria);

            // Assert
            Assert.AreEqual(expected.Error, actual.Error);
            Assert.AreEqual(expected.IsSuccessful, actual.IsSuccessful);
            Assert.AreEqual(expected.Message, actual.Message);
        }

        [TestMethod]
        public async Task ExecuteNonQueryAsync_CriteriaIsValid_ExpectedExecuteQuery()
        {
            // Arrange
            var criteria = Substitute.For<INonQueryCommand>();
            criteria.IsValid().Returns(true);
            var baseService = GetBaseService();

            // Act
            await baseService.ExecuteNonQueryAsync(criteria);

            // Assert
            await baseRepository.Received().ExecuteNonQueryAsync(Arg.Is<INonQueryCommand>(x => x.IsValid()));
        }

        private BaseService GetBaseService()
        {
            dynamicRepository = Substitute.For<IDynamicRepository>();
            baseRepository = Substitute.For<IBaseRepository>();
            var service = new BaseService(dynamicRepository, baseRepository);

            return service;
        }
    }
}
