using AutoFixture;
using Ploutos.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Ploutos.Common.Tests.Models
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
            fixture.Create<CommandResult>();
            fixture.Create<IdentifyModel>();
            fixture.Create<ProxySettings>();
            fixture.Create<RedisConfig>();
        }
    }
}
