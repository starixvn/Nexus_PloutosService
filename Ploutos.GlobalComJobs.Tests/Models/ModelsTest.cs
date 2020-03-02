using AutoFixture;
using Ploutos.GlobalComJobs.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Ploutos.GlobalComJobs.Tests.Models
{
    public class ModelsTest
    {
        private Fixture fixture;


        [Fact]
        public void Models_Create_WithoutError()
        {
            // Arrange
            fixture = new Fixture();

            // Act & Assert
            fixture.Create<ApiConfigs>();
            fixture.Create<ApiParam>();
            fixture.Create<AuthData>();
            fixture.Create<AuthParam>();
            fixture.Create<BetTicket>();
            fixture.Create<BetTicketBase>();
            fixture.Create<BetTicketToPush>();
            fixture.Create<BrandApiConfigs>();
            fixture.Create<TicketResponse>();
            fixture.Create<UpdateBetTicket>();
            fixture.Create<UrlApiConfig>();
        }
    }
}
