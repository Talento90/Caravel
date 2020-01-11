namespace Caravel.AspNetCore.Middleware
{
    public class TraceIdOptions
    {
        public const string DefaultHeader = "X-Trace-ID";

        /// <summary>
        /// The header field name where the Trace Id will be stored
        /// </summary>
        public string Header { get; set; } = DefaultHeader;

        /// <summary>
        /// Controls whether the trace id is returned in the response headers
        /// </summary>
        public bool IncludeInResponse { get; set; } = true;
    }
}