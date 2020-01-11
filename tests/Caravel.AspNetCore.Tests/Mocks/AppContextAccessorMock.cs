using System;
using Caravel.AppContext;

namespace Caravel.Tests.Mocks
{
    public class AppContextAccessorMock : IAppContextAccessor
    {
        public AppContext.AppContext Context { get; set; }

        public AppContextAccessorMock()
        {
            Context = new AppContext.AppContext(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
        }
        
        public AppContextAccessorMock(string traceId, string uid)
        {
            Context = new AppContext.AppContext(traceId, uid);
        }
    }
}