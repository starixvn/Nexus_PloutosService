namespace Ploutos.WebAPI.UnitTests.Controller
{
    using AutoFixture;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using NSubstitute;
    using Ploutos.WebAPI.Controllers;
    using Ploutos.WebAPI.Models;
    using Ploutos.WebAPI.Services.Interfaces;
    using System.Linq;
    using System.Threading.Tasks;

    [TestClass]
    public class TicketControllerTests
    {
        private readonly Fixture fixture = new Fixture();

        [TestMethod]
        public async Task InsertTickets_WhenCalled_ReturnResultExpect()
        {
            // Arrange
            var ticketService = Substitute.For<ITicketService>();
            var controller = new TicketController(ticketService);
            var betTickets = fixture.CreateMany<BetTicket>().ToList();

            // Act
            var result = await controller.InsertTickets(betTickets);

            //Assert
            Assert.IsNull(result);
        }
    }
}
