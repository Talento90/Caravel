using System.Collections.Generic;

namespace Caravel.AppContext
{
    public class AppContext
    {
        public string? UserId { get; }
        public string TraceId { get; }
        public IDictionary<string, string> Data { get; }

        public AppContext(string cid, string? userId)
        {
            UserId = userId;
            TraceId = cid;
            Data = new Dictionary<string, string>();
        }
    }
}