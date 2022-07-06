using System;
using GodelTech.XUnit.Logging.Tests.Fakes;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace GodelTech.XUnit.Logging.Tests
{
    public class TestLoggerTests
    {
        [Fact]
        public void Log_WhenIsNotEnabled_DoNothing()
        {
            // Arrange
            var logger = new TestLogger(null, null, null, string.Empty, false);

            // Act
            logger.Log<FakeTestLogEntryState>(LogLevel.None, new EventId(), null, null, null);

            // Assert
            Assert.NotNull(logger);
        }

        [Fact]
        public void Log_WhenFormatterIsNull_ThrowsArgumentNullException()
        {
            // Arrange
            var logger = new TestLogger(null, null, null, string.Empty, false);

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(
                () => logger.Log<FakeTestLogEntryState>(LogLevel.Critical, new EventId(), null, null, null)
            );

            Assert.Equal("formatter", exception.ParamName);
        }

        [Fact]
        public void Log_WhenOutputNotNull_WriteLine()
        {
            // Arrange
            var state = new FakeTestLogEntryState
            {
                Name = "Test Name"
            };

            var exception = new ArgumentNullException();

            Func<object, Exception, string> formatter = (obj, e) => $"{((FakeTestLogEntryState) obj).Name} - {e.GetType()}";

            var mockTestOutputHelper = new Mock<ITestOutputHelper>(MockBehavior.Strict);
            mockTestOutputHelper
                .Setup(x => x.WriteLine(formatter(state, exception)));

            var logger = new TestLogger(mockTestOutputHelper.Object, null, null, string.Empty, false);

            // Act
            logger.Log<FakeTestLogEntryState>(LogLevel.Critical, new EventId(), state, exception, formatter);

            // Assert
            mockTestOutputHelper
                .Verify(
                    x => x.WriteLine(formatter(state, exception)),
                    Times.Once
                );
        }

        [Fact]
        public void Log_WhenTestLoggerContextAccessorNotNull_AddEntry()
        {
            // Arrange
            var eventId = new EventId(1, "Test Name");

            var state = new FakeTestLogEntryState
            {
                Name = "Test Name"
            };

            var exception = new ArgumentNullException();

            static string formatter(object obj, Exception e)
            {
                return $"{((FakeTestLogEntryState) obj).Name} - {e.GetType()}";
            }

            var testLoggerContextAccessor = new TestLoggerContextAccessor();

            var logger = new TestLogger(null, testLoggerContextAccessor, null, "Test CategoryName", true);

            // Act
            logger.Log<FakeTestLogEntryState>(LogLevel.Critical, eventId, state, exception, (Func<object, Exception, string>) formatter);

            // Assert
            var entry = Assert.Single(testLoggerContextAccessor.TestLoggerContext.Entries);
            Assert.Equal(LogLevel.Critical, entry.LogLevel);
            Assert.Equal(eventId, entry.EventId);
            Assert.Equal(state, entry.State);
            Assert.Equal(exception, entry.Exception);
            Assert.Equal(formatter(state, exception), entry.Formatter(state, exception));
            Assert.Equal("Test CategoryName", entry.CategoryName);
            Assert.True(entry.UsesScopes);
        }

        [Theory]
        [InlineData(LogLevel.Trace, true)]
        [InlineData(LogLevel.Debug, true)]
        [InlineData(LogLevel.Information, true)]
        [InlineData(LogLevel.Warning, true)]
        [InlineData(LogLevel.Error, true)]
        [InlineData(LogLevel.Critical, true)]
        [InlineData(LogLevel.None, false)]
        public void IsEnabled_Success(
            LogLevel logLevel,
            bool expectedResult)
        {
            // Arrange
            var logger = new TestLogger(null, null, null, string.Empty, false);

            // Act
            var result = logger.IsEnabled(logLevel);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void BeginScope_Success()
        {
            // Arrange
            var state = new FakeTestLogEntryState
            {
                Name = "Test Name"
            };

            using var disposable = new FakeDisposable();

            var mockExternalScopeProvider = new Mock<IExternalScopeProvider>(MockBehavior.Strict);
            mockExternalScopeProvider
                .Setup(x => x.Push(state))
                .Returns(disposable);

            var logger = new TestLogger(null, null, mockExternalScopeProvider.Object, string.Empty, false);

            // Act
            var result = logger.BeginScope(state);

            // Assert
            mockExternalScopeProvider
                .Verify(
                    x => x.Push(state),
                    Times.Once
                );

            Assert.Equal(disposable, result);
        }
    }
}
