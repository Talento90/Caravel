using System.Collections.Generic;
using System.Linq;
 
 namespace Caravel.AspNetCore.Middleware
 {
     /// <summary>
     /// LoggingOptions is used to configure the <see cref="LoggingMiddleware"/>.
     /// </summary>
     public class LoggingSettings
     {
         /// <summary>
         /// Headers to log. Default: "Content-Type", "User-Agent"
         /// </summary>
         public IEnumerable<string> HeadersToLog { get; set; }
         
         /// <summary>
         /// Paths to ignore. Default: "/docs", "/swagger", "/health", "/api/version"
         /// </summary>
         public IEnumerable<string> PathsToIgnore { get; set; }
         /// <summary>
         /// Paths to ignore the body.
         /// This can be used to avoid logging sensitive endpoints.
         /// </summary>
         public IEnumerable<string> PathsToRedact { get; set; }
         
         /// <summary>
         /// Enable logging the body. Default: false
         /// </summary>
         public bool EnableLogBody { get; set; }
 
         public LoggingSettings()
         {
             PathsToIgnore = new[] {"/docs", "/swagger", "/health", "/api/version"};
             PathsToRedact = Enumerable.Empty<string>();
             HeadersToLog = new[] {"Content-Type", "User-Agent"};
             EnableLogBody = false;
         }
     }
 }