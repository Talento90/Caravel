using System.Collections.Generic;
using System.Security.Claims;

namespace Caravel.AspNetCore.Middleware
{
    /// <summary>
    /// AppContextEnricherSettings is used to configure the <see cref="AppContextEnricherMiddleware"/>.
    /// </summary>
    public class AppContextEnricherSettings
    {
        /// <summary>
        /// List of claims to inject in <see cref="AppContext"/> Data.
        /// </summary>
        public IEnumerable<Claim> Claims { get; set; }

        public AppContextEnricherSettings()
        {
            Claims = new List<Claim>();
        }
    }
}