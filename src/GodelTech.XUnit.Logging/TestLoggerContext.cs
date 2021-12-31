using System.Collections.Generic;

namespace GodelTech.XUnit.Logging
{
    /// <summary>
    /// Encapsulates all Logger specific information.
    /// </summary>
    public class TestLoggerContext
    {
        /// <summary>
        /// The Log entries.
        /// </summary>
        public IList<TestLogEntry> Entries { get; } = new List<TestLogEntry>();
    }
}