using System;
using Xunit;

namespace GodelTech.XUnit.Logging.Tests
{
    public class LoggingBuilderHelpersTests
    {
        [Fact]
        public void UsesScopes_ThrowsArgumentNullException()
        {
            // Arrange & Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(
                () => LoggingBuilderHelpers.UsesScopes(null)
            );

            Assert.Equal("builder", exception.ParamName);
        }
    }
}