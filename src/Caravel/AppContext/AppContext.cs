using System.Collections.Generic;

namespace Caravel.AppContext
{
    public class AppContext
    {
        public string? UserId { get; }
        public string CorrelationId { get; }
        public IDictionary<string, string> Data { get; }

        public AppContext(string cid, string? userId)
        {
            UserId = userId;
            CorrelationId = cid;
            Data = new Dictionary<string, string>();
        }
    }
}