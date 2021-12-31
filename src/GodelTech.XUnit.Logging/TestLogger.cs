using Microsoft.Extensions.Logging;
using System;
using Xunit.Abstractions;

[assembly: CLSCompliant(false)]
namespace GodelTech.XUnit.Logging
{
    /// <summary>
    /// Represents a type used to perform logging.
    /// </summary>
    public class TestLogger : ILogger
    {
        private readonly ITestOutputHelper _output;
        private readonly ITestLoggerContextAccessor _testLoggerContextAccessor;
        private readonly IExternalScopeProvider _scopeProvider;
        private readonly string _categoryName;
        private readonly bool _usesScopes;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestLogger"/> class.
        /// </summary>
        /// <param name="output">The Test output.</param>
        /// <param name="testLoggerContextAccessor">The <see cref="ITestLoggerContextAccessor"/>.</param>
        /// <param name="scopeProvider">The scope provider.</param>
        /// <param name="categoryName">The category name.</param>
        /// <param name="usesScopes">Indicates if logging has scopes.</param>
        public TestLogger(
            ITestOutputHelper output,
            ITestLoggerContextAccessor testLoggerContextAccessor,
            IExternalScopeProvider scopeProvider,
            string categoryName,
            bool usesScopes)
        {
            _output = output;
            _testLoggerContextAccessor = testLoggerContextAccessor;
            _scopeProvider = scopeProvider;
            _categoryName = categoryName;
            _usesScopes = usesScopes;
        }

        /// <inheritdoc />
        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception exception,
            Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel)) return;

            if (formatter == null) throw new ArgumentNullException(nameof(formatter));

            if (_output != null)
            {
                _output.WriteLine(formatter.Invoke(state, exception));
            }

            if (_testLoggerContextAccessor != null)
            {
                var entry = new TestLogEntry(
                    logLevel,
                    eventId,
                    state,
                    exception,
                    (obj, e) => formatter.Invoke((TState) obj, e),
                    _categoryName,
                    _usesScopes
                );

                _testLoggerContextAccessor.TestLoggerContext.Entries.Add(entry);
            }
        }

        /// <inheritdoc />
        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel != LogLevel.None;
        }

        /// <inheritdoc />
        public IDisposable BeginScope<TState>(TState state)
        {
            return _scopeProvider.Push(state);
        }
    }
}