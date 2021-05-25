namespace Ploutos.Common.Tests.Utils
{
    using System;
    using System.Collections.Generic;
    using FluentAssertions;
    using Ploutos.Common.Const;
    using Ploutos.Common.Models;
    using Ploutos.Common.Utils;
    using Xunit;

    public class UtilTest
    {
        [Fact]
        public void SplitToBatches_WhenCalled_SplitDataIntoBatches()
        {
            // Arrange
            var data = new List<int> { 1, 2, 3, 4, 5 };
            var batchSize = 2;
            var util = new Util();

            var expectedResult = new List<List<int>> {
                new List<int> { 1, 2 },
                new List<int> { 3, 4 },
                new List<int> { 5 }
            };

            // Act
            var result = util.SplitToBatches<int>(data, batchSize);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }


        [Fact]
        public void ConvertJsonResponse_WhenCalled_ConvertJson()
        {
            // Arrange
            string jsons = @"{
                              'UserName': 'test',
                              'Password': 1234
                            }";
            var util = new Util();

            // Act
            var result = util.ConvertJsonResponse<IdentifyModel>(jsons);

            // Assert
            Assert.Equal("test", result.UserName);
            Assert.Equal("1234", result.Password);
        }

        [Fact]
        public void GetCurrentTimeAtGMTMinus4_WhenCalled_ReturnTimeExpected()
        {
            // Arrange
            var util = new Util();
            DateTime currentTime = DateTime.UtcNow.AddHours(Consts.GmtMinus4);

            // Act
            var result = util.GetCurrentTimeAtGMTMinus4();

            Exception exception = new Exception();

           var results = util.BuildLogMessage(exception);

            // Assert
            Assert.Equal(currentTime.Date, result.Date);
            Assert.Equal(currentTime.Hour, result.Hour);
            Assert.Equal(currentTime.Minute, result.Minute);
        }
    }
}
