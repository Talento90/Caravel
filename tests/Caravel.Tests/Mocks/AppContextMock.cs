using System;
using Caravel.AppContext;

namespace Caravel.Tests.Mocks
{
    public class AppContextMock : IAppContext
    {
        public AppContext.AppContext Context { get; set; }

        public AppContextMock()
        {
            Context = new AppContext.AppContext(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());
        }
        
        public AppContextMock(string cid, string uid)
        {
            Context = new AppContext.AppContext(cid, uid);
        }
    }
}