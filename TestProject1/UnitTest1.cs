using FluentAssertions;
using Microsoft.VisualBasic.ApplicationServices;
using PrivateMethodAccess.Authorization;
using System.Security.Claims;

namespace TestProject1
{
    public class UnitTest1
    {
        [Fact]
        public void TestUnauthenticatedUser()
        {
            //var user = TestClaimsPrincipalFactory.CreateUnauthenticatedUser();
            //var provider = new SecureActionProvider(user);
            //var action = provider.GetAuthorizedAction();
            //var result = action();

            // Arrange            
            var user = TestClaimsPrincipalFactory.CreateUnauthenticatedUser();
            var provider = new SecureActionProvider(user);

            // Act            
            Delegate authDelegate = provider.GetAuthorizedAction();

            // Assert
            
        }
    }

    
}