namespace Ploutos.Common.Tests.Repository
{
    using Microsoft.Extensions.Options;
    using NSubstitute;
    using Ploutos.Common.Models;
    using Ploutos.Common.Repository;
    using System.Threading.Tasks;
    using Xunit;

    public class RedisRepositoryTests
    {
        private readonly IOptions<RedisConfig> redisConfig;

        public RedisRepositoryTests()
        {
            redisConfig = Substitute.For<IOptions<RedisConfig>>();
        }

        [Fact]
        public void Get_WhenCalled_ReturnResultExpect()
        {
            // Arrange
            var key = "SyncOpal88Player";
            var configs = new RedisConfig
            {
                Host = "ha.nexdev.net:16379,password=foo1@bared,defaultDatabase=7",
                Namespace = "PloutosService"
            };
            redisConfig.Value.Returns(configs);
            var redisRepository = new RedisRepository(redisConfig) { };

            // Act
            var result = redisRepository.Get(key);

            // Assert
            Assert.Equal(TaskStatus.WaitingForActivation, result.Status);
        }

        [Fact]
        public void Set_WhenCalled_ReturnResultExpect()
        {
            // Arrange
            var key = "xx";
            var configs = new RedisConfig
            {
                Host = "ha.nexdev.net:16379,password=foo1@bared,defaultDatabase=7",
                Namespace = "PloutosService"
            };
            redisConfig.Value.Returns(configs);
            string value = null;
            var redisRepository = new RedisRepository(redisConfig) { };

            // Act
            var result = redisRepository.Set(key, value);

            // Assert
            Assert.Equal(TaskStatus.WaitingForActivation, result.Status);
        }
    }
}

