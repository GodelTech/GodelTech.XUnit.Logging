using System;
using GodelTech.XUnit.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Logging extensions.
    /// </summary>
    public static class LoggingExtensions
    {
        /// <summary>
        /// Adds test logging.
        /// </summary>
        /// <param name="builder">The WebHost builder.</param>
        /// <param name="output">The Test output.</param>
        /// <param name="testLoggerContextAccessor">The <see cref="ITestLoggerContextAccessor"/>.</param>
        /// <returns>Returns WebHost builder.</returns>
        public static IWebHostBuilder UseTestLogging(
            this IWebHostBuilder builder,
            ITestOutputHelper output,
            ITestLoggerContextAccessor testLoggerContextAccessor)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            builder.ConfigureLogging(
                loggingBuilder =>
                {
                    // check if scopes are used in normal operation
                    var usesScopes = loggingBuilder.UsesScopes();

                    // remove other logging providers, such as remote loggers or unnecessary event logs
                    loggingBuilder.ClearProviders();

                    loggingBuilder.Services.TryAddSingleton(testLoggerContextAccessor);
                    loggingBuilder.AddProvider(
                        new TestLoggerProvider(
                            output,
                            testLoggerContextAccessor,
                            usesScopes
                        )
                    );
                }
            );

            return builder;
        }
    }
}