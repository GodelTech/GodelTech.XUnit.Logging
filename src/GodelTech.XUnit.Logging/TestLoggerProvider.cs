using System;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace GodelTech.XUnit.Logging
{
    /// <summary>
    /// Represents a type that can create instances of <see cref="Microsoft.Extensions.Logging.ILogger" />.
    /// </summary>
    public class TestLoggerProvider : ILoggerProvider, ISupportExternalScope
    {
        private readonly ITestOutputHelper _output;
        private readonly ITestLoggerContextAccessor _testLoggerContextAccessor;
        private readonly bool _usesScopes;

        private IExternalScopeProvider _scopeProvider;
        private bool _isDisposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestLoggerProvider"/> class.
        /// </summary>
        /// <param name="output">The Test output.</param>
        /// <param name="testLoggerContextAccessor">The <see cref="ITestLoggerContextAccessor"/>.</param>
        /// <param name="usesScopes">Indicates if logging has scopes.</param>
        public TestLoggerProvider(
            ITestOutputHelper output,
            ITestLoggerContextAccessor testLoggerContextAccessor,
            bool usesScopes)
        {
            _output = output;
            _testLoggerContextAccessor = testLoggerContextAccessor;
            _usesScopes = usesScopes;
        }

        /// <inheritdoc />
        public ILogger CreateLogger(string categoryName)
        {
            return new TestLogger(
                _output,
                _testLoggerContextAccessor,
                _scopeProvider,
                categoryName,
                _usesScopes
            );
        }

        /// <inheritdoc />
        public void SetScopeProvider(IExternalScopeProvider scopeProvider)
        {
            _scopeProvider = scopeProvider;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #region Dispose

        /// <summary>
        /// If disposing equals true, the method has been called directly
        /// or indirectly by a user's code. Managed and unmanaged resources
        /// are disposed.
        /// If disposing equals false, the method has been called by the
        /// runtime from inside the finalizer: only unmanaged resources can be disposed.
        /// </summary>
        /// <param name="disposing">
        /// Indicates, whether method has been
        /// called directly by user code from Dispose() or from Finalizer.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
            {
                // unmanaged resources would be cleaned up here.
                return;
            }

            if (_isDisposed)
            {
                // no need to dispose twice.
                return;
            }

            // free managed resources
            _isDisposed = true;
        }

        #endregion
    }
}