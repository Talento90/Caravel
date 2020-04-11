namespace Caravel.Exceptions
{
    /// <summary>
    /// Error class represents the application error model.
    /// All errors should use this class in order to provide consistency.
    /// </summary>
    public sealed class Error
    {
        /// <summary>
        /// Unique error code.
        /// </summary>
        public int Code { get; }
        /// <summary>
        /// Error message.
        /// </summary>
        public string Message { get; }
        /// <summary>
        /// Error Severity.
        /// </summary>
        public Severity Severity { get; }

        public Error(int code, string message, Severity severity = Severity.Low)
        {
            Code = code;
            Message = message;
            Severity = severity;
        }
    }
}