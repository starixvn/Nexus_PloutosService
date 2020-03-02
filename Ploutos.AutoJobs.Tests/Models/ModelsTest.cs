using AutoFixture;
using Ploutos.AutoJobs.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Ploutos.AutoJobs.Tests.Models
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
            fixture.Create<BetTicket>();
            fixture.Create<BrandApiConfigs>();
            fixture.Create<MailConfigs>();
            fixture.Create<MailModel>();
            fixture.Create<PlayerProfile>();
            fixture.Create<ProfileResponse>();
            fixture.Create<ProfileResponseBadRequest>();
            fixture.Create<ProfileResponseData>();
            fixture.Create<ResponseError>();
            fixture.Create<ResponseMessage>();
            fixture.Create<SupportApiAuthObj>();
            fixture.Create<SupportApiConfigs>();
            fixture.Create<TicketResponse>();
            fixture.Create<TicketResponseBadRequest>();
            fixture.Create<TicketResponseData>();
            fixture.Create<UpdateBetTicket>();
            fixture.Create<UpdatePlayerProfile>();
            fixture.Create<UrlApiConfig>();            
        }
    }
}
