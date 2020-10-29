namespace Ploutos.WebAPI.UnitTests.Repositories
{
    using Fanex.Data.Repository;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using NSubstitute;
    using Ploutos.WebAPI.Repositories;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [TestClass]
    public class BaseRepositoryTests
    {
        private IDynamicRepository dynamicRepository;

        public BaseRepositoryTests()
        {
        }

        [TestMethod]
        public async Task GetDataAsync_CriteriaIsNull_ReturnEmptyList()
        {
            // Arrange
            var baseRepository = GetBaseRepository();

            // Act
            var actual = await baseRepository.GetDataAsync<int>(null);

            // Assert
            Assert.AreEqual(actual.Count(), 0);
        }

        [TestMethod]
        public async Task GetDataAsync_CriteriaIsInvalid_ReturnEmptyList()
        {
            // Arrange
            var criteria = Substitute.For<ICriteria>();
            criteria.IsValid().Returns(false);
            var baseRepository = GetBaseRepository();

            // Act
            var actual = await baseRepository.GetDataAsync<int>(criteria);

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
            var baseRepository = GetBaseRepository();
            dynamicRepository.FetchAsync<int>(criteria).Returns(expected);

            // Act
            var actual = await baseRepository.GetDataAsync<int>(criteria);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public async Task FetchMultipleAsync_WithThreeDataSetCriteriaIsNull_ReturnTupleWithEmptyList()
        {
            // Arrange
            var baseRepository = GetBaseRepository();

            // Act
            var actual = await baseRepository.FetchMultipleAsync<int, int, int>(null);

            // Assert
            Assert.AreEqual(actual.Item1.Count(), 0);
            Assert.AreEqual(actual.Item2.Count(), 0);
            Assert.AreEqual(actual.Item3.Count(), 0);
        }

        [TestMethod]
        public async Task FetchMultipleAsync_WithThreeDataCriteriaIsInvalid_ReturnTupleWithEmptyList()
        {
            // Arrange
            var criteria = Substitute.For<ICriteria>();
            criteria.IsValid().Returns(false);
            var baseRepository = GetBaseRepository();

            // Act
            var actual = await baseRepository.FetchMultipleAsync<int, int, int>(criteria);

            // Assert
            Assert.AreEqual(actual.Item1.Count(), 0);
            Assert.AreEqual(actual.Item2.Count(), 0);
            Assert.AreEqual(actual.Item3.Count(), 0);
        }

        [TestMethod]
        public async Task FetchMultipleAsync_WithThreeDataCriteriaIsValid_ReturnExpectedData()
        {
            // Arrange
            var expected = new ValueTuple<IEnumerable<int>, IEnumerable<int>, IEnumerable<int>>(new List<int> { 1, 2 }, new List<int> { 1, 2 }, new List<int> { 1, 2 });
            var criteria = Substitute.For<ICriteria>();
            criteria.IsValid().Returns(true);
            var baseRepository = GetBaseRepository();
            dynamicRepository.FetchMultipleAsync<int, int, int>(criteria).Returns(expected);

            // Act
            var actual = await baseRepository.FetchMultipleAsync<int, int, int>(criteria);

            // Assert
            Assert.AreEqual(expected, actual);
        }

        private static INonQueryCommand FakeCriteria()
        {
            var criteria = Substitute.For<INonQueryCommand>();

            criteria.GetSettingKey().Returns("key");
            criteria.IsValid().Returns(true);

            return criteria;
        }

        private BaseRepository GetBaseRepository()
        {
            dynamicRepository = Substitute.For<IDynamicRepository>();
            var repository = new BaseRepository(dynamicRepository);

            return repository;
        }
    }
}
