using System.Collections.Generic;

namespace Caravel.AppContext
{
    /// <summary>
    /// Encapsulates all Application specific information about an individual operation.
    /// </summary>
    public class AppContext
    {
        /// <summary>
        /// Get the current user id.
        /// </summary>
        public string? UserId { get; }
        /// <summary>
        /// Get the current trace id. 
        /// </summary>
        public string TraceId { get; }
        /// <summary>
        /// Get a dictionary that contains request related data. 
        /// </summary>
        public IDictionary<string, object> Data { get; }

        public AppContext(string traceId, string? userId)
        {
            UserId = userId;
            TraceId = traceId;
            Data = new Dictionary<string, object>();
        }
    }
}