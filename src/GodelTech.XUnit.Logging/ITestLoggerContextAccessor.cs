namespace GodelTech.XUnit.Logging
{
    /// <summary>
    /// Provides access to the current <see cref="TestLoggerContext"/>, if one is available.
    /// </summary>
    /// <remarks>
    /// This interface should be used with caution. It relies on <see cref="System.Threading.AsyncLocal{T}" /> which can have a negative performance impact on async calls.
    /// It also creates a dependency on "ambient state" which can make testing more difficult.
    /// </remarks>
    public interface ITestLoggerContextAccessor
    {
        /// <summary>
        /// Gets or sets the current <see cref="TestLoggerContext"/>. Returns <see langword="null" /> if there is no active <see cref="TestLoggerContext" />.
        /// </summary>
        TestLoggerContext TestLoggerContext { get; set; }
    }
}