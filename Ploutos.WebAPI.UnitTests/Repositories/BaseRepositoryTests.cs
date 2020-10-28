//namespace Ploutos.WebAPI.UnitTests.Repositories
//{
//    using AutoFixture;
//    using Microsoft.VisualStudio.TestTools.UnitTesting;
//    using NSubstitute;
//    using Ploutos.WebAPI.Criteria.Ticket;
//    using Ploutos.WebAPI.Repositories;
//    using System;
//    using System.Collections;
//    using System.Threading.Tasks;

//    [TestClass]
//    public class BaseRepositoryTests
//    {
//        private readonly IObjectDb objectDb;
//        private readonly Fixture fixture;

//        public BaseRepositoryTests()
//        {
//            DbSettingProviderManager.StartTestingSession().Run();
//            objectDb = Substitute.For<IObjectDb>();
//            ObjectDbFactory.Mock((key, time) => objectDb);
//            fixture = new Fixture();
//        }

//        [TestMethod]
//        public async Task ExecuteNonQueryAsync_WhenCalled_ExecuteNonQueryAsync()
//        {
//            // Arrange
//            var repository = new BaseRepository();
//            var command = new InsertBetTicketsCriteria();


//            // Act
//            await repository.ExecuteNonQueryAsync(command);

//            // Assert
//            await objectDb.Received().ExecuteNonQueryAsync(command);
//        }

//        [TestMethod]
//        public async Task ExecuteNonQueryAsync_WhenNoError_ReturnEmpty()
//        {
//            // Arrange
//            var repository = new BaseRepository();
//            var command = new InsertBetTicketsCriteria();

//            var hash = new Hashtable
//            {
//                { "Error", DBNull.Value },
//                { "Message", DBNull.Value }
//            };
//            objectDb.GetOutputParams().Returns(hash);

//            // Act
//            var result = await repository.ExecuteNonQueryAsync(command);

//            // Assert
//            result.Error.Should().Be(0);
//            result.Message.Should().BeEmpty();
//        }

//        [TestMethod]
//        public async Task ExecuteNonQueryAsync_WhenError_ReturnErrors()
//        {
//            // Arrange
//            var repository = new BaseRepository();
//            var command = new InsertBetTicketsCriteria();

//            var hash = new Hashtable();
//            var errorCode = 123;
//            var errorMsg = "some error";
//            hash.Add("Error", errorCode);
//            hash.Add("Message", errorMsg);
//            objectDb.GetOutputParams().Returns(hash);

//            // Act
//            var result = await repository.ExecuteNonQueryAsync(command);

//            // Assert
//            result.Error.Should().Be(errorCode);
//            result.Message.Should().Be(errorMsg);
//        }
//    }
//}
