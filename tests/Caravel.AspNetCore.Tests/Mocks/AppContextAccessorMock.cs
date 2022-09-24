using System;
using Caravel.ApplicationContext;

namespace Caravel.AspNetCore.Tests.Mocks
{
    public class AppContextAccessorMock : IAppContextAccessor
    {
        public ApplicationContext.ApplicationContext Context { get; set; }

        public AppContextAccessorMock()
        {
            Context = new ApplicationContext.ApplicationContext(Guid.NewGuid().ToString(), Guid.NewGuid(),Guid.NewGuid());
        }
        
        public AppContextAccessorMock(string traceId, Guid uid, Guid tenantId)
        {
            Context = new ApplicationContext.ApplicationContext(traceId, uid, tenantId);
        }
    }
}