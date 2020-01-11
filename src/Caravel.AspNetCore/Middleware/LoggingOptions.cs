using System.Collections.Generic;
 using System.Linq;
 
 namespace Caravel.AspNetCore.Middleware
 {
     public class LoggingOptions
     {
         public IEnumerable<string> HeadersToLog { get; set; }
         public IEnumerable<string> PathsToIgnore { get; set; }
         public IEnumerable<string> PathsToRedact { get; set; }
         public bool EnableLogBody { get; set; }
 
         public LoggingOptions()
         {
             PathsToIgnore = new[] {"/docs", "/swagger", "/health", "/api/version"};
             PathsToRedact = Enumerable.Empty<string>();
             HeadersToLog = new[] {"Content-Type", "User-Agent"};
             EnableLogBody = false;
         }
     }
 }