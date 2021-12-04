namespace GodelTech.XUnit.Logging
{
    /// <summary>
    /// Provides an implementation of <see cref="ITestLoggerContextAccessor" />.
    /// </summary>
    public class TestLoggerContextAccessor : ITestLoggerContextAccessor
    {
        private TestLoggerContextHolder _testLoggerContextCurrent = new TestLoggerContextHolder();

        /// <summary>
        /// Initializes a new instance of the <see cref="TestLoggerContextAccessor"/> class.
        /// </summary>
        public TestLoggerContextAccessor()
        {
            TestLoggerContext = new TestLoggerContext();
        }

        /// <inheritdoc/>
        public TestLoggerContext TestLoggerContext
        {
            get
            {
                return _testLoggerContextCurrent?.Context;
            }
            set
            {
                var holder = _testLoggerContextCurrent;
                if (holder != null)
                {
                    // Clear current TestLoggerContext trapped in the AsyncLocals, as its done.
                    holder.Context = null;
                }

                if (value != null)
                {
                    // Use an object indirection to hold the TestLoggerContext in the AsyncLocal,
                    // so it can be cleared in all ExecutionContexts when its cleared.
                    _testLoggerContextCurrent = new TestLoggerContextHolder { Context = value };
                }
            }
        }

        private sealed class TestLoggerContextHolder
        {
            public TestLoggerContext Context;
        }
    }
}