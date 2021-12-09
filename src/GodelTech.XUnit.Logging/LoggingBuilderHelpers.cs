using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace GodelTech.XUnit.Logging
{
    /// <summary>
    /// LoggingBuilder helpers.
    /// </summary>
    public static class LoggingBuilderHelpers
    {
        /// <summary>
        /// Checks if logging has scopes.
        /// </summary>
        /// <param name="builder">Logging builder.</param>
        /// <returns>True if logging uses scopes.</returns>
        public static bool UsesScopes(this ILoggingBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException(nameof(builder));

            var serviceProvider = builder.Services.BuildServiceProvider();

            // look for other host builders on this chain calling ConfigureLogging explicitly
            var options = serviceProvider.GetService<SimpleConsoleFormatterOptions>()
                          ?? serviceProvider.GetService<JsonConsoleFormatterOptions>()
                          ?? serviceProvider.GetService<ConsoleFormatterOptions>();

            if (options != default) return options.IncludeScopes;

            // look for other configuration sources
            // See: https://docs.microsoft.com/en-us/dotnet/core/extensions/logging?tabs=command-line#set-log-level-by-command-line-environment-variables-and-other-configuration
            var config = serviceProvider.GetService<IConfigurationRoot>()
                         ?? serviceProvider.GetService<IConfiguration>();

            var logging = config?.GetSection("Logging");

            if (logging == default) return false;

            var includeScopes = logging.GetValue("Console:IncludeScopes", false);

            if (!includeScopes)
            {
                includeScopes = logging.GetValue("IncludeScopes", false);
            }

            return includeScopes;
        }
    }
}