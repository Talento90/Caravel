namespace Caravel.AspNetCore.Middleware
{
    /// <summary>
    /// TraceIdOptions is used to configure the <see cref="TraceIdMiddleware"/>.
    /// </summary>
    public class TraceIdSettings
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