using System;
using Caravel.ApplicationContext;

namespace Caravel.AspNetCore.Tests.Mocks;

public class ApplicationContextAccessorMock : IApplicationContextAccessor
{
    public ApplicationContext.ApplicationContext Context { get; set; }

    public ApplicationContextAccessorMock()
    {
        Context = new ApplicationContext.ApplicationContext(Guid.NewGuid().ToString(), Guid.NewGuid(), Guid.NewGuid());
    }

    public ApplicationContextAccessorMock(string traceId, Guid uid, Guid tenantId)
    {
        Context = new ApplicationContext.ApplicationContext(traceId, uid, tenantId);
    }
}