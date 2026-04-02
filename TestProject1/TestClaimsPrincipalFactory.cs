using System.Security.Claims;

namespace TestProject1;

public static class TestClaimsPrincipalFactory
{
    // 1. Unauthenticated user (no identity)
    public static ClaimsPrincipal CreateUnauthenticatedUser()
    {
        return new ClaimsPrincipal(new ClaimsIdentity());
    }

    // 2. Authenticated regular user (no special roles/claims)
    public static ClaimsPrincipal CreateRegularUser(string username = "JohnDoe")
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Email, $"{username}@example.com")
        };

        var identity = new ClaimsIdentity(claims, "TestAuth");
        return new ClaimsPrincipal(identity);
    }

    // 3. Admin user with Admin role
    public static ClaimsPrincipal CreateAdminUser(string username = "AdminUser")
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Email, $"{username}@example.com"),
            new Claim(ClaimTypes.Role, "Admin")
        };

        var identity = new ClaimsIdentity(claims, "TestAuth");
        return new ClaimsPrincipal(identity);
    }

    // 4. User with specific permission claim
    public static ClaimsPrincipal CreateUserWithPermission(string permission, string username = "PermUser")
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
            new Claim("Permission", permission)
        };

        var identity = new ClaimsIdentity(claims, "TestAuth");
        return new ClaimsPrincipal(identity);
    }

    // 5. User with multiple roles
    public static ClaimsPrincipal CreateMultiRoleUser(string username = "PowerUser")
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role, "Manager"),
            new Claim(ClaimTypes.Role, "Editor"),
            new Claim("SecureAccessLevel", "5")
        };

        var identity = new ClaimsIdentity(claims, "TestAuth");
        return new ClaimsPrincipal(identity);
    }

    // 6. Null user (simulates no user context)
    public static ClaimsPrincipal? CreateNullUser()
    {
        return null;
    }
}
