using FluentAssertions;
using NSubstitute;
using Ploutos.Common.Repository;
using Ploutos.Common.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Ploutos.Common.Tests.Services
{
    public class JobRunningCheckerTest
    {
        private readonly IRedisRepository redisRepository;

        public JobRunningCheckerTest()
        {
            redisRepository = Substitute.For<IRedisRepository>();
        }

        [Fact]
        public async Task IsRunning_WhenValueTRUE_ReturnTrue()
        {
            // Arrange
            var jobRunningChecker = new JobRunningChecker(redisRepository);
            var jobName = "xx";
            redisRepository.Get(Arg.Any<string>()).Returns("true");

            // Act
            var result = await jobRunningChecker.IsRunning(jobName);

            // Assert
            result.Should().Be(true);
        }

        [Fact]
        public async Task IsRunning_WhenValueNotTRUE_ReturnFalse()
        {
            // Arrange
            var jobRunningChecker = new JobRunningChecker(redisRepository);
            var jobName = "xx";
            redisRepository.Get(Arg.Any<string>()).Returns("truex");

            // Act
            var result = await jobRunningChecker.IsRunning(jobName);

            // Assert
            result.Should().Be(false);
        }

        [Fact]
        public async Task MarkRunning_WhenCalled_UpdateValueTrue()
        {
            // Arrange
            var jobRunningChecker = new JobRunningChecker(redisRepository);
            var jobName = "xx";

            string value = null;
            await redisRepository.Set(Arg.Any<string>(), Arg.Do<string>(x => value = x));

            // Act
            await jobRunningChecker.MarkRunning(jobName);

            // Assert
            value.Should().Be("true");
        }

        [Fact]
        public async Task ClearRunning_WhenCalled_UpdateValueFalse()
        {
            // Arrange
            var jobRunningChecker = new JobRunningChecker(redisRepository);
            var jobName = "xx";

            string value = null;
            await redisRepository.Set(Arg.Any<string>(), Arg.Do<string>(x => value = x));

            // Act
            await jobRunningChecker.ClearRunning(jobName);

            // Assert
            value.Should().Be("false");
        }
    }
}
