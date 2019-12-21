namespace Caravel.Exceptions
{
    public sealed class Error
    {
        public int Code { get; }
        public string Message { get; }
        public Severity Severity { get; }

        public Error(int code, string message, Severity severity = Severity.Low)
        {
            Code = code;
            Message = message;
            Severity = severity;
        }
    }
}