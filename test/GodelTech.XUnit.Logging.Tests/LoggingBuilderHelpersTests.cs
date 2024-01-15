using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Moq;
using Xunit;

namespace GodelTech.XUnit.Logging.Tests
{
    public class LoggingBuilderHelpersTests
    {
        private readonly Mock<ILoggingBuilder> _mockLoggingBuilder;
        private readonly IServiceCollection _serviceCollection;

        public LoggingBuilderHelpersTests()
        {
            _mockLoggingBuilder = new Mock<ILoggingBuilder>(MockBehavior.Strict);
            _serviceCollection = new ServiceCollection();

            _mockLoggingBuilder
                .Setup(x => x.Services)
                .Returns(_serviceCollection);
        }

        [Fact]
        public void UsesScopes_ThrowsArgumentNullException()
        {
            // Arrange & Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(
                () => LoggingBuilderHelpers.UsesScopes(null)
            );

            Assert.Equal("builder", exception.ParamName);
        }

        public static IEnumerable<object[]> ConsoleFormatterOptionsMemberData =>
            new Collection<object[]>
            {
                // AddSimpleConsole
                new object[]
                {
                    new Action<ILoggingBuilder>(
                        builder => builder.AddSimpleConsole()
                    ),
                    false
                },
                new object[]
                {
                    new Action<ILoggingBuilder>(
                        builder => builder.AddSimpleConsole(
                            options => options.IncludeScopes = false
                        )
                    ),
                    false
                },
                new object[]
                {
                    new Action<ILoggingBuilder>(
                        builder => builder.AddSimpleConsole(
                            options => options.IncludeScopes = true
                        )
                    ),
                    true
                },
                // AddJsonConsole
                new object[]
                {
                    new Action<ILoggingBuilder>(
                        builder => builder.AddJsonConsole()
                    ),
                    false
                },
                new object[]
                {
                    new Action<ILoggingBuilder>(
                        builder => builder.AddJsonConsole(
                            options => options.IncludeScopes = false
                        )
                    ),
                    false
                },
                new object[]
                {
                    new Action<ILoggingBuilder>(
                        builder => builder.AddJsonConsole(
                            options => options.IncludeScopes = true
                        )
                    ),
                    true
                },
                // AddSystemdConsole
                new object[]
                {
                    new Action<ILoggingBuilder>(
                        builder => builder.AddSystemdConsole()
                    ),
                    false
                },
                new object[]
                {
                    new Action<ILoggingBuilder>(
                        builder => builder.AddSystemdConsole(
                            options => options.IncludeScopes = false
                        )
                    ),
                    false
                },
                new object[]
                {
                    new Action<ILoggingBuilder>(
                        builder => builder.AddSystemdConsole(
                            options => options.IncludeScopes = true
                        )
                    ),
                    true
                },
                // AddConsole
                new object[]
                {
                    new Action<ILoggingBuilder>(
                        builder => builder.AddConsole()
                    ),
                    false
                }
            };

        [Theory]
        [MemberData(nameof(ConsoleFormatterOptionsMemberData))]
        public void UsesScopes_WithConsoleFormatterOptions_Success(
            Action<ILoggingBuilder> configureLoggingBuilder,
            bool expectedResult)
        {
            // Arrange
            configureLoggingBuilder(_mockLoggingBuilder.Object);

            // Act
            var result = _mockLoggingBuilder.Object.UsesScopes();

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void UsesScopes_WhenConsoleFormatterOptionsIsNull_ReturnsFalse()
        {
            // Arrange
            _serviceCollection.Configure<ConsoleLoggerOptions>(options => options.FormatterName = "test");

            // Act
            var result = _mockLoggingBuilder.Object.UsesScopes();

            // Assert
            Assert.False(result);
        }

        public static IEnumerable<object[]> ConfigurationMemberData =>
            new Collection<object[]>
            {
                new object[]
                {
                    new ConfigurationBuilder()
                        .Build(),
                    false
                },
                new object[]
                {
                    new ConfigurationBuilder()
                        .AddInMemoryCollection(
                            new Dictionary<string, string>
                            {
                                {"Logging", ""}
                            }
                        )
                        .Build(),
                    false
                },
                new object[]
                {
                    new ConfigurationBuilder()
                        .AddInMemoryCollection(
                            new Dictionary<string, string>
                            {
                                {"Logging:Console:IncludeScopes", bool.FalseString}
                            }
                        )
                        .Build(),
                    false
                },
                new object[]
                {
                    new ConfigurationBuilder()
                        .AddInMemoryCollection(
                            new Dictionary<string, string>
                            {
                                {"Logging:Console:IncludeScopes", bool.TrueString}
                            }
                        )
                        .Build(),
                    true
                },
                new object[]
                {
                    new ConfigurationBuilder()
                        .AddInMemoryCollection(
                            new Dictionary<string, string>
                            {
                                {"Logging:IncludeScopes", bool.FalseString}
                            }
                        )
                        .Build(),
                    false
                },
                new object[]
                {
                    new ConfigurationBuilder()
                        .AddInMemoryCollection(
                            new Dictionary<string, string>
                            {
                                {"Logging:IncludeScopes", bool.TrueString}
                            }
                        )
                        .Build(),
                    true
                }
            };

        [Theory]
        [MemberData(nameof(ConfigurationMemberData))]
        public void UsesScopes_WithConfigurationRoot_Success(
            IConfigurationRoot configuration,
            bool expectedResult)
        {
            // Arrange
            _serviceCollection.AddSingleton(configuration);

            // Act
            var result = _mockLoggingBuilder.Object.UsesScopes();

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [MemberData(nameof(ConfigurationMemberData))]
        public void UsesScopes_WithConfiguration_Success(
            IConfiguration configuration,
            bool expectedResult)
        {
            // Arrange
            _serviceCollection.AddSingleton(configuration);

            // Act
            var result = _mockLoggingBuilder.Object.UsesScopes();

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void UsesScopes_WithConfigurations_Success()
        {
            // Arrange
            var configurationRoot = new ConfigurationBuilder()
                .AddInMemoryCollection(
                    new Dictionary<string, string>
                    {
                        {"Logging:Console:IncludeScopes", bool.TrueString}
                    }
                )
                .Build();

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(
                    new Dictionary<string, string>
                    {
                        {"Logging:Console:IncludeScopes", bool.FalseString}
                    }
                )
                .Build();

            _serviceCollection.AddSingleton(configurationRoot);
            _serviceCollection.AddSingleton<IConfiguration>(configuration);

            // Act
            var result = _mockLoggingBuilder.Object.UsesScopes();

            // Assert
            Assert.True(result);
        }
    }
}
