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
        private readonly TestLoggerProvider _provider;

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

            // Act & Assert
            _provider.SetScopeProvider(mockExternalScopeProvider.Object);

            // Assert
            Assert.NotNull(_provider);
        }

        [Fact]
        public void Dispose_WithFalse()
        {
            // Arrange
            var fakeProvider = (FakeTestLoggerProvider) _provider;

            // Act
            fakeProvider.ExposedDispose(false);

            // Assert
            Assert.NotNull(fakeProvider);
        }

        [Fact]
        public void Dispose_WhenIsDisposed()
        {
            // Arrange
            var fakeProvider = (FakeTestLoggerProvider) _provider;

            // Act
            fakeProvider.Dispose();

            fakeProvider.ExposedDispose(true);

            // Assert
            Assert.NotNull(fakeProvider);
        }

        [Fact]
        public void Dispose_WhenDbContextIsNull()
        {
            // Arrange
            using var fakeProvider = new FakeTestLoggerProvider(null, null, false);

            // Act
            fakeProvider.ExposedDispose(true);

            // Assert
            Assert.NotNull(fakeProvider);
        }
    }
}
