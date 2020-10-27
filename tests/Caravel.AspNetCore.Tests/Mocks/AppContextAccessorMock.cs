using System;
using Caravel.AppContext;

namespace Caravel.AspNetCore.Tests.Mocks
{
    public class AppContextAccessorMock : IAppContextAccessor
    {
        public AppContext.AppContext Context { get; set; }

        public AppContextAccessorMock()
        {
            Context = new AppContext.AppContext(Guid.NewGuid().ToString(), Guid.NewGuid(),Guid.NewGuid());
        }
        
        public AppContextAccessorMock(string traceId, Guid uid, Guid tenantId)
        {
            Context = new AppContext.AppContext(traceId, uid, tenantId);
        }
    }
}