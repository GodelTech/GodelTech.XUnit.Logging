using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;

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
            var consoleLoggerOptions = serviceProvider.GetService<IOptions<ConsoleLoggerOptions>>();

            if (consoleLoggerOptions != null)
            {
                IOptions<ConsoleFormatterOptions> consoleFormatterOptions = null;

                switch (consoleLoggerOptions.Value.FormatterName)
                {
                    case ConsoleFormatterNames.Simple:
                    {
                        consoleFormatterOptions = serviceProvider.GetService<IOptions<SimpleConsoleFormatterOptions>>();
                        break;
                    }
                    case ConsoleFormatterNames.Json:
                    {
                        consoleFormatterOptions = serviceProvider.GetService<IOptions<JsonConsoleFormatterOptions>>();
                        break;
                    }
                    case ConsoleFormatterNames.Systemd:
                    {
                        consoleFormatterOptions = serviceProvider.GetService<IOptions<ConsoleFormatterOptions>>();
                        break;
                    }
                }

                if (consoleFormatterOptions != null)
                {
                    return consoleFormatterOptions.Value.IncludeScopes;
                }
            }

            // look for other configuration sources
            // See: https://docs.microsoft.com/en-us/dotnet/core/extensions/logging?tabs=command-line#set-log-level-by-command-line-environment-variables-and-other-configuration
            var config = serviceProvider.GetService<IConfigurationRoot>()
                         ?? serviceProvider.GetService<IConfiguration>();

            var logging = config?.GetSection("Logging");

            if (logging == null) return false;

            var includeScopes = logging.GetValue("Console:IncludeScopes", false);

            if (!includeScopes)
            {
                includeScopes = logging.GetValue("IncludeScopes", false);
            }

            return includeScopes;
        }
    }
}