using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace GodelTech.XUnit.Logging.Tests.Fakes
{
    public class FakeTestLoggerProvider : TestLoggerProvider
    {
        public FakeTestLoggerProvider(
            ITestOutputHelper output,
            ITestLoggerContextAccessor testLoggerContextAccessor,
            bool usesScopes)
            : base(output, testLoggerContextAccessor, usesScopes)
        {

        }

        public IExternalScopeProvider ExposedScopeProvider => ScopeProvider;

        public bool ExposedIsDisposed => IsDisposed;

        public void ExposedDispose(bool disposing)
        {
            Dispose(disposing);
        }
    }
}
