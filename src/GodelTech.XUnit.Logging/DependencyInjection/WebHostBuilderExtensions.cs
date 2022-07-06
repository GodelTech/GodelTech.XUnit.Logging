using GodelTech.XUnit.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// WebHostBuilder extensions.
    /// </summary>
    public static class WebHostBuilderExtensions
    {
        /// <summary>
        /// Enables the <see cref="TestLogger" />.
        /// </summary>
        /// <param name="builder">The <see cref="IWebHostBuilder" />.</param>
        /// <param name="output">The <see cref="ITestOutputHelper" />.</param>
        /// <param name="testLoggerContextAccessor">The <see cref="ITestLoggerContextAccessor"/>.</param>
        /// <returns>The <see cref="IWebHostBuilder"/>.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("SonarQube", "S4792: Make sure that this logger's configuration is safe.", Justification = "By design.")]
        public static IWebHostBuilder ConfigureTestLogging(
            this IWebHostBuilder builder,
            ITestOutputHelper output = null,
            ITestLoggerContextAccessor testLoggerContextAccessor = null)
        {
            builder.ConfigureLogging(
                loggingBuilder =>
                {
                    // check if scopes are used in normal operation
                    var usesScopes = loggingBuilder.UsesScopes();

                    // remove other logging providers, such as remote loggers or unnecessary event logs
                    loggingBuilder.ClearProviders();

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
