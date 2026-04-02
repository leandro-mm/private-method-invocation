using FluentAssertions;
using PrivateMethodAccess;

namespace TestProject1
{
    public class UnitTest1
    {
        private FormSecureService _form;
        public UnitTest1()
        {
            _form = new FormSecureService();
        }

        [Fact]
        public void Test_Admin_User()
        {
            // Arrange            
            var user = TestClaimsPrincipalFactory.CreateAdminUser();
            var role = TestClaimsPrincipalFactory.GetClaimRole(user);           

            // Act
            Action action = () => _form.GetAuthorizedDelegate(role);
                        
            // Assert
            action.Should().NotThrow();
        }

        [Fact]
        public void Test_Unauthenticated_User()
        {
            // Arrange            
            var user = TestClaimsPrincipalFactory.CreateUnauthenticatedUser();
            var role = TestClaimsPrincipalFactory.GetClaimRole(user);

            // Act
            Action action = () => _form.GetAuthorizedDelegate(role);

            // Assert
            action.Should().Throw();
        }

        [Fact]
        public void Test_Regular_User()
        {
            // Arrange            
            var user = TestClaimsPrincipalFactory.CreateRegularUser();
            var role = TestClaimsPrincipalFactory.GetClaimRole(user);

            // Act
            Action action = () => _form.GetAuthorizedDelegate(role);

            // Assert
            action.Should().Throw();
        }

        [Fact]
        public void Test_User_With_Permission()
        {
            // Arrange            
            var user = TestClaimsPrincipalFactory.CreateUserWithPermission("Admin");
            var role = TestClaimsPrincipalFactory.GetClaimRole(user);

            // Act
            Action action = () => _form.GetAuthorizedDelegate(role);

            // Assert
            action.Should().Throw();
        }

        [Fact]
        public void Test_Multi_Role_User()
        {
            // Arrange            
            var user = TestClaimsPrincipalFactory.CreateMultiRoleUser();
            var role = TestClaimsPrincipalFactory.GetClaimRole(user);

            // Act
            Action action = () => _form.GetAuthorizedDelegate(role);

            // Assert
            action.Should().Throw();
        }

        [Fact]
        public void Test_Null_User()
        {
            // Arrange            
            var user = TestClaimsPrincipalFactory.CreateNullUser();
            var role = TestClaimsPrincipalFactory.GetClaimRole(user);

            // Act
            Action action = () => _form.GetAuthorizedDelegate(role);

            // Assert
            action.Should().Throw();
        }
    }

    
}