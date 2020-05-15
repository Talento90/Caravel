namespace Caravel.Errors
{
    /// <summary>
    /// Error Severity
    /// </summary>
    public enum Severity
    {
        /// <summary>
        /// Low severity is used for cases that this error DOES NOT
        /// cause any real impact on the application.
        ///
        /// Information Log Level
        /// </summary>
        Low,
        /// <summary>
        /// Medium severity is used for the cases where the error can
        /// have partial or limited impact on the application but not critical.
        ///
        /// Warning Log Level
        /// </summary>
        Medium,
        /// <summary>
        /// High severity is used whenever the error compromises the application.
        /// E.g Database is not down.
        ///
        /// Error Log Level
        /// </summary>
        High
    }
}