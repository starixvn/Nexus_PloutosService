namespace Ploutos.WebAPI.UnitTests.Repositories
{
    using AutoFixture;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using NSubstitute;
    using Ploutos.WebAPI.Criteria.Ticket;
    using Ploutos.WebAPI.Models;
    using Ploutos.WebAPI.Repositories;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [TestClass]
    public class TicketRepositoryTests
    {
        private readonly Fixture fixture = new Fixture();

        [TestMethod]
        public async Task InserBetTickets_WhenCalled_InserBetTickets()
        {
            // Arrange
            var baseRepository = Substitute.For<IBaseRepository>();
            var ticketRepository = new TicketRepository(baseRepository);
            var betTickets = fixture.CreateMany<BetTicket>().ToList();

            InsertBetTicketsCriteria criteria = null;
            await baseRepository.ExecuteNonQueryAsync(Arg.Do<InsertBetTicketsCriteria>(x => criteria = x));

            // Act
            await ticketRepository.InserBetTickets(betTickets);

            // Assert
            Assert.IsFalse(string.IsNullOrEmpty(criteria.InsertCommand));
        }

        [TestMethod]
        public async Task GetDuplicateTicketData_WhenCalled_GetDuplicateTicketData()
        {
            // Arrange
            var baseRepository = Substitute.For<IBaseRepository>();
            var ticketRepository = new TicketRepository(baseRepository);
            var betTickets = fixture.CreateMany<BetTicket>().ToList();

            GetDuplicateTicketDataCriteria criteria = null;
            await baseRepository.GetDataAsync<string>(Arg.Do<GetDuplicateTicketDataCriteria>(x => criteria = x));

            // Act
            var result = await ticketRepository.GetDuplicateTicketData(betTickets);

            // Assert
            Assert.IsFalse(string.IsNullOrEmpty(criteria.TransactionNoList));
        }

        [TestMethod]
        public async Task GetTicketSettings_WhenCalled_ReturnExpectedData()
        {
            // Arrange
            var baseRepository = Substitute.For<IBaseRepository>();
            var ticketRepository = new TicketRepository(baseRepository);
            var ticketSettingData = 
                new ValueTuple<IEnumerable<string>, IEnumerable<string>, IEnumerable<string>>(new List<string>(), new List<string>(), new List<string>());
            var returnData = new TicketSettingData
            {
                GamePlatformCodeList = ticketSettingData.Item1.ToList(),
                GamePlatformProductCodeList = ticketSettingData.Item2.ToList(),
                UserAccountNoList = ticketSettingData.Item3.ToList()
            };

            baseRepository.FetchMultipleAsync<string, string, string>(Arg.Any<GetTicketSettingCriteria>()).Returns(ticketSettingData);

            // Act
            var result = await ticketRepository.GetTicketSettings();

            // Assert
            Assert.AreEqual(result.GamePlatformCodeList.Count, returnData.GamePlatformCodeList.Count);
            Assert.AreEqual(result.GamePlatformProductCodeList.Count, returnData.GamePlatformProductCodeList.Count);
            Assert.AreEqual(result.UserAccountNoList.Count, returnData.UserAccountNoList.Count);
        }
    }
}
