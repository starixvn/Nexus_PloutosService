using FluentAssertions;
using Ploutos.Common.Models;
using Ploutos.Common.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Ploutos.Common.Tests.Utils
{
    public class UtilTest
    {
        [Fact]
        public void SerializeList_WhenListEmpty_ReturnEmpty()
        {
            // Arrange
            var list = new List<string>();
            var util = new Util();

            // Act
            var result = util.SerializeList<string>(list);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void SerializeList_WhenListNotEmpty_ReturnSerializedString()
        {
            // Arrange
            var list = new List<IdentifyModel> {
                new IdentifyModel { UserName = "a" },
                new IdentifyModel { UserName = "c" }
            };
            var util = new Util();

            var expectedResult = "<root><IdentifyModel><UserName>a</UserName></IdentifyModel><IdentifyModel><UserName>c</UserName></IdentifyModel></root>";

            // Act
            var result = util.SerializeList<IdentifyModel>(list);

            // Assert
            result.Should().Be(expectedResult);
        }

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
    }
}
