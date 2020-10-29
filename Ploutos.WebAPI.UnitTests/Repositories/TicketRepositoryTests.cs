namespace Ploutos.WebAPI.UnitTests.Repositories
{
    using AutoFixture;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using NSubstitute;
    using Ploutos.WebAPI.Criteria.Ticket;
    using Ploutos.WebAPI.Models;
    using Ploutos.WebAPI.Repositories;
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
    }
}
