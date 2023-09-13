using System;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace GodelTech.XUnit.Logging.Tests.Fakes
{
    public class FakeTestLoggerProvider : TestLoggerProvider
    {
        private readonly Action<bool> _onDispose;
        private readonly Action<bool> _onAfterDispose;

        public FakeTestLoggerProvider(
            ITestOutputHelper output,
            ITestLoggerContextAccessor testLoggerContextAccessor,
            bool usesScopes,
            Action<bool> onDispose = null,
            Action<bool> onAfterDispose = null)
            : base(output, testLoggerContextAccessor, usesScopes)
        {
            _onDispose = onDispose;
            _onAfterDispose = onAfterDispose;
        }

        public IExternalScopeProvider ExposedScopeProvider => ScopeProvider;

        public bool ExposedIsDisposed => IsDisposed;

        public void ExposedDispose(bool disposing)
        {
            Dispose(disposing);
        }

        protected override void Dispose(bool disposing)
        {
            _onDispose?.Invoke(disposing);

            base.Dispose(disposing);

            _onAfterDispose?.Invoke(IsDisposed);
        }
    }
}
