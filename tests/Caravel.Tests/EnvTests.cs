using System;
using Xunit;

namespace Caravel.Tests
{
    public class EnvTests : IDisposable
    {
        [Fact]
        public void Should_Be_Local_Environment()
        {
            // Arrange
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Local");
            
            //Act
            var env = Env.IsLocalEnvironment();
            
            //Assert
            Assert.True(env);
        }
        
        [Fact]
        public void Should_Be_Dev_Environment()
        {
            // Arrange
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Development");
            
            //Act
            var env = Env.IsDevelopmentEnvironment();
            
            //Assert
            Assert.True(env);
        }
        
        [Fact]
        public void Should_Be_Test_Environment()
        {
            // Arrange
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Test");
            
            //Act
            var env = Env.IsTestEnvironment();
            
            //Assert
            Assert.True(env);
        }
        
        [Fact]
        public void Should_Be_Sandbox_Environment()
        {
            // Arrange
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Sandbox");
            
            //Act
            var env = Env.IsSandboxEnvironment();
            
            //Assert
            Assert.True(env);
        }
        
        [Fact]
        public void Should_Be_Production_Environment()
        {
            // Arrange
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Production");
            
            //Act
            var env = Env.IsProductionEnvironment();
            
            //Assert
            Assert.True(env);
        }
        
        public void Dispose()
        {
            Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", string.Empty);
        }
    }
}