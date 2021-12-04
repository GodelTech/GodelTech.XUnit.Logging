using System;
using Microsoft.Extensions.Logging;

namespace GodelTech.XUnit.Logging
{
    /// <summary>
    /// Log entry model.
    /// </summary>
    public class TestLogEntry
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestLogEntry"/> class.
        /// </summary>
        /// <param name="logLevel">Entry will be written on this level.</param>
        /// <param name="eventId">Id of the event.</param>
        /// <param name="state">The entry to be written. Can be also an object.</param>
        /// <param name="exception">The exception related to this entry.</param>
        /// <param name="formatter">Function to create a <see cref="System.String" /> message of the <paramref name="state" /> and <paramref name="exception" />.</param>
        public TestLogEntry(
            LogLevel logLevel,
            EventId eventId,
            object state,
            Exception exception,
            Func<object, Exception, string> formatter)
        {
            LogLevel = logLevel;
            EventId = eventId;
            State = state;
            Exception = exception;
            Formatter = formatter;
        }

        /// <summary>
        /// Entry will be written on this level.
        /// </summary>
        public LogLevel LogLevel { get; }

        /// <summary>
        /// Id of the event.
        /// </summary>
        public EventId EventId { get; }

        /// <summary>
        /// The entry to be written. Can be also an object.
        /// </summary>
        public object State { get; }

        /// <summary>
        /// The exception related to this entry.
        /// </summary>
        public Exception Exception { get; }

        /// <summary>
        /// Function to create a <see cref="System.String" /> message of the State and Exception.
        /// </summary>
        public Func<object, Exception, string> Formatter { get; }
    }
}