using Xunit;

namespace GodelTech.XUnit.Logging.Tests
{
    public class TestLoggerContextTests
    {
        [Fact]
        public void Entries_Get_Success()
        {
            // Arrange
            var context = new TestLoggerContext();

            // Act
            var result = context.Entries;

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}