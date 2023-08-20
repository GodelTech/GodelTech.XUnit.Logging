using System;
using GodelTech.XUnit.Logging.Tests.Fakes;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace GodelTech.XUnit.Logging.Tests
{
    public sealed class TestLoggerProviderTests : IDisposable
    {
        private readonly FakeTestLoggerProvider _provider;

        public TestLoggerProviderTests()
        {
            var mockTestOutputHelper = new Mock<ITestOutputHelper>(MockBehavior.Strict);
            var mockTestLoggerContextAccessor = new Mock<ITestLoggerContextAccessor>(MockBehavior.Strict);

            _provider = new FakeTestLoggerProvider(
                mockTestOutputHelper.Object,
                mockTestLoggerContextAccessor.Object,
                false
            );
        }

        public void Dispose()
        {
            _provider.Dispose();
        }

        [Fact]
        public void CreateLogger_Success()
        {
            // Arrange & Act
            var result = _provider.CreateLogger("Test CategoryName");

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void SetScopeProvider_Success()
        {
            // Arrange
            var mockExternalScopeProvider = new Mock<IExternalScopeProvider>(MockBehavior.Strict);

            // Act
            _provider.SetScopeProvider(mockExternalScopeProvider.Object);

            // Assert
            Assert.Equal(mockExternalScopeProvider.Object, _provider.ExposedScopeProvider);
        }

        [Fact]
        public void Dispose_Success()
        {
            // Arrange
            FakeTestLoggerProvider provider;

            // Act & Assert
            using (provider = new FakeTestLoggerProvider(null, null, false))
            {
                Assert.False(provider.ExposedIsDisposed);
            }

            Assert.True(provider.ExposedIsDisposed);
        }

        [Fact]
        public void Dispose_WhenIsDisposed()
        {
            // Arrange & Act
            _provider.Dispose();

            _provider.ExposedDispose(true);

            // Assert
            Assert.True(_provider.ExposedIsDisposed);
        }

        [Fact]
        public void Dispose_WithFalse()
        {
            // Arrange & Act
            _provider.ExposedDispose(false);

            // Assert
            Assert.True(_provider.ExposedIsDisposed);
        }
    }
}
