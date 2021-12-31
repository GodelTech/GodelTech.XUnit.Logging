using Xunit;

namespace GodelTech.XUnit.Logging.Tests
{
    public class TestLoggerContextAccessorTests
    {
        private readonly TestLoggerContextAccessor _accessor;

        public TestLoggerContextAccessorTests()
        {
            _accessor = new TestLoggerContextAccessor();
        }

        [Fact]
        public void TestLoggerContext_Get_IsNotNull()
        {
            // Arrange & Act & Assert
            Assert.NotNull(_accessor.TestLoggerContext);
            Assert.Empty(_accessor.TestLoggerContext.Entries);
        }

        [Fact]
        public void TestLoggerContext_Set_Success()
        {
            // Arrange
            var context = new TestLoggerContext();

            // Act
            _accessor.TestLoggerContext = context;

            // Assert
            Assert.Equal(context, _accessor.TestLoggerContext);
        }

        [Fact]
        public void TestLoggerContext_SetNull_ReturnsNull()
        {
            // Arrange
            var context = new TestLoggerContext();

            _accessor.TestLoggerContext = context;

            // Act
            _accessor.TestLoggerContext = null;

            // Assert
            Assert.Null(_accessor.TestLoggerContext);
        }
    }
}