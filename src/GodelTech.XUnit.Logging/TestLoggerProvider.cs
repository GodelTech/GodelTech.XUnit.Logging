using System;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace GodelTech.XUnit.Logging
{
    /// <summary>
    /// Represents a type that can create instances of <see cref="ILogger" />.
    /// </summary>
    public class TestLoggerProvider : ILoggerProvider, ISupportExternalScope
    {
        private readonly ITestOutputHelper _output;
        private readonly ITestLoggerContextAccessor _testLoggerContextAccessor;
        private readonly bool _usesScopes;

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

        /// <summary>
        /// Scope provider.
        /// </summary>
        protected IExternalScopeProvider ScopeProvider { get; private set; }

        /// <summary>
        /// Is disposed.
        /// </summary>
        protected bool IsDisposed { get; private set; }

        /// <inheritdoc />
        public ILogger CreateLogger(string categoryName)
        {
            return new TestLogger(
                _output,
                _testLoggerContextAccessor,
                ScopeProvider,
                categoryName,
                _usesScopes
            );
        }

        /// <inheritdoc />
        public void SetScopeProvider(IExternalScopeProvider scopeProvider)
        {
            ScopeProvider = scopeProvider;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            // Stryker disable once boolean
            Dispose(true);
            // Stryker disable once statement
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
            // Stryker disable once block
            if (IsDisposed)
            {
                // no need to dispose twice.
                // Stryker disable once statement
                return;
            }

            // Stryker disable once boolean
            if (disposing)
            {
                // dispose managed state (managed objects)
            }

            // free unmanaged resources (unmanaged objects) and override finalizer
            // set large fields to null
            IsDisposed = true;
        }

        #endregion
    }
}
