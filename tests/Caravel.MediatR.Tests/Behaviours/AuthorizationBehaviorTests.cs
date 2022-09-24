using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Caravel.ApplicationContext;
using Caravel.MediatR.Behaviours;
using Caravel.MediatR.Security;
using FluentValidation;
using Moq;
using Xunit;

namespace Caravel.MediatR.Tests.Behaviours
{
    public class AuthorizationBehaviorTests
    {
        [Fact]
        public async Task Should_Throw_Authorization_Exception_When_User_Is_Null()
        {
            var query = new GetTestDataQuery();
            var ctx = new AppContextAccessor()
            {
                Context = new ApplicationContext.ApplicationContext("")
            };
            
            var mock = new Mock<IAuthorizer>();

            var behaviour = new AuthorizationBehaviour<GetTestDataQuery, TestDataResponse>(ctx, mock.Object);

            var ex = await Assert.ThrowsAsync<Exceptions.UnauthorizedException>(() =>
                behaviour.Handle(query, CancellationToken.None, () => Task.FromResult(new TestDataResponse()))
            );
            
            Assert.Equal("authentication", ex.Error.Code);
        }
        
        [Fact]
        public async Task Should_Throw_Permission_Exception_When_User_Role_Is_Missing()
        {
            var userId = Guid.NewGuid();
            var query = new GetTestDataQuery();
            var ctx = new AppContextAccessor()
            {
                Context = new ApplicationContext.ApplicationContext("", userId)
            };
            
            var mock = new Mock<IAuthorizer>();

            mock.Setup(s => s.IsInRoleAsync(userId, "admin", CancellationToken.None)).ReturnsAsync(false);
            mock.Setup(s => s.AuthorizeAsync(userId, "CanRead", CancellationToken.None)).ReturnsAsync(false);;

            var behaviour = new AuthorizationBehaviour<GetTestDataQuery, TestDataResponse>(ctx, mock.Object);

            var ex = await Assert.ThrowsAsync<Exceptions.PermissionException>(() =>
                behaviour.Handle(query, CancellationToken.None, () => Task.FromResult(new TestDataResponse()))
            );
            
            Assert.Equal("permission", ex.Error.Code);
        }
        
        [Fact]
        public async Task Should_Throw_Permission_Exception_When_User_Policy_Is_Missing()
        {
            var userId = Guid.NewGuid();
            var query = new GetTestDataQuery();
            var ctx = new AppContextAccessor()
            {
                Context = new ApplicationContext.ApplicationContext("", userId)
            };
            
            var mock = new Mock<IAuthorizer>();

            mock.Setup(s => s.IsInRoleAsync(userId, "admin", CancellationToken.None)).ReturnsAsync(true);
            mock.Setup(s => s.AuthorizeAsync(userId, "CanRead", CancellationToken.None)).ReturnsAsync(false);;

            var behaviour = new AuthorizationBehaviour<GetTestDataQuery, TestDataResponse>(ctx, mock.Object);

            var ex = await Assert.ThrowsAsync<Exceptions.PermissionException>(() =>
                behaviour.Handle(query, CancellationToken.None, () => Task.FromResult(new TestDataResponse()))
            );
            
            Assert.Equal("permission", ex.Error.Code);
        }

        [Fact]
        public async Task Should_Authorize()
        {
            var userId = Guid.NewGuid();
            var query = new GetTestDataQuery();
            var ctx = new AppContextAccessor()
            {
                Context = new ApplicationContext.ApplicationContext("", userId)
            };
            
            var mock = new Mock<IAuthorizer>();

            mock.Setup(s => s.IsInRoleAsync(userId, "admin", CancellationToken.None)).ReturnsAsync(true);
            mock.Setup(s => s.AuthorizeAsync(userId, "CanRead", CancellationToken.None)).ReturnsAsync(true);;

            var behaviour = new AuthorizationBehaviour<GetTestDataQuery, TestDataResponse>(ctx, mock.Object);

            var result = await behaviour.Handle(query, CancellationToken.None,
                () => Task.FromResult(new TestDataResponse(){Data = "banana"}));
            
            Assert.Equal("banana", result.Data);
        }
    }
}