using System;
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
        public Guid? UserId { get; }
        /// <summary>
        /// Get the tenant id. This field is useful for multi tenant applications/
        /// </summary>
        public Guid? TenantId { get; }
        /// <summary>
        /// Get the current trace id. 
        /// </summary>
        public string TraceId { get; }
        /// <summary>
        /// Get a dictionary that contains request related data. 
        /// </summary>
        public IDictionary<string, object> Data { get; }

        public AppContext(string traceId, Guid? userId = default, Guid? tenantId = default)
        {
            UserId = userId;
            TenantId = tenantId;
            TraceId = traceId;
            Data = new Dictionary<string, object>();
        }
    }
}