namespace Ploutos.WebAPI.UnitTests.Criteria.Player
{
    using Ploutos.WebAPI.Criteria.Player;
    using Xunit;

    public class GetUserAccountNumberByUsernameCriteriaTests
    {
        [Fact]
        public void GetSettingKey_ReturnCorrectSettingKey()
        {
            // Arrange
            var criteria = new GetUserAccountNumberByUsernameCriteria();
            var expected = "Player_GetUserAccountNumberByUsername";

            // Act
            var actual = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void IsValid_ParamsAreInvalid_ReturnFalse(string username)
        {
            // Arrange
            var criteria = new GetUserAccountNumberByUsernameCriteria
            {
                Username = username
            };
            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.False(actual);
        }

        [Fact]
        public void IsValid_ParamsArevalid_ReturnTrue()
        {
            // Arrange
            var criteria = new GetUserAccountNumberByUsernameCriteria
            {
                Username = "test"
            };
            // Act
            var actual = criteria.IsValid();

            // Assert
            Assert.True(actual);
        }
    }
}
