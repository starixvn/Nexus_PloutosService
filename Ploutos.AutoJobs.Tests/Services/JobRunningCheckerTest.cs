using FluentAssertions;
using NSubstitute;
using Ploutos.AutoJobs.Services;
using Ploutos.Common.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Ploutos.AutoJobs.Tests.Services
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

        [Fact]
        public async Task ClearAllRunning_WhenCalled_ClearAll()
        {
            // Arrange
            var jobRunningChecker = new JobRunningChecker(redisRepository);

            string value1 = null;
            string value2 = null;
            await redisRepository.Set(Arg.Do<string>(x => value1 = x), Arg.Do<string>(x => value2 = x));

            // Act
            await jobRunningChecker.ClearAllRunning();

            // Assert
            await redisRepository.Received().Set(string.Format("PloutosService:RunningJobs:{0}", Constant.Constants.SyncOpal88Player), "false");
            await redisRepository.Received().Set(string.Format("PloutosService:RunningJobs:{0}", Constant.Constants.SyncOpal88Ticket), "false");
            await redisRepository.Received().Set(string.Format("PloutosService:RunningJobs:{0}", Constant.Constants.SyncNova88Player), "false");
            await redisRepository.Received().Set(string.Format("PloutosService:RunningJobs:{0}", Constant.Constants.SyncNova88Ticket), "false");
            await redisRepository.Received().Set(string.Format("PloutosService:RunningJobs:{0}", Constant.Constants.SyncIvorybetPlayer), "false");
            await redisRepository.Received().Set(string.Format("PloutosService:RunningJobs:{0}", Constant.Constants.SyncIvorybetTicket), "false");
        }
    }
}
