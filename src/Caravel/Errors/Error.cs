namespace Caravel.Errors
{
    /// <summary>
    /// Error class represents the application error model.
    /// All errors should use this class in order to provide consistency.
    /// </summary>
    public class Error
    {
        /// <summary>
        /// Unique error code.
        /// </summary>
        public string Code { get; }
        /// <summary>
        /// Error message.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Error Severity.
        /// </summary>
        public Severity Severity { get; }
        
        /// <summary>
        /// Error Details.
        /// </summary>
        public string? Details { get; private set; }

        public Error(string code, string message, Severity severity = Severity.Low)
        { 
            Code = code;
            Message = message;
            Severity = severity;
        }

        public Error SetDetails(string details)
        {
            Details = details;

            return this;
        }
    }
}