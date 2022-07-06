using System;
using GodelTech.XUnit.Logging.Tests.Fakes;
using Microsoft.Extensions.Logging;
using Xunit;

namespace GodelTech.XUnit.Logging.Tests
{
    public class TestLogEntryTests
    {
        [Fact]
        public void Constructor_Success()
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

            // Act
            var result = new TestLogEntry(
                LogLevel.Critical,
                eventId,
                state,
                exception,
                formatter,
                "Test CategoryName",
                true
            );

            // Assert
            Assert.NotNull(result);
            Assert.Equal(LogLevel.Critical, result.LogLevel);
            Assert.Equal(eventId, result.EventId);
            Assert.Equal(state, result.State);
            Assert.Equal(exception, result.Exception);
            Assert.Equal(formatter, result.Formatter);
            Assert.Equal("Test CategoryName", result.CategoryName);
            Assert.True(result.UsesScopes);
            Assert.Equal(formatter(state, exception), result.Message);
        }
    }
}
