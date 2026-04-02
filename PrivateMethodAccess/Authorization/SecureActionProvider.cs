using System.Security.Claims;

namespace PrivateMethodAccess.Authorization;

public class SecureActionProvider
{
    private readonly ClaimsPrincipal _user;
    
    public delegate string AuthorizedAction();

    private bool IsUserAuthorized
    {
        get
        {
            return _user?.Identity != null && _user.IsInRole("Admin");
        }
    }

    

    public SecureActionProvider(ClaimsPrincipal user)
    {
        _user = user;   
    }

    public AuthorizedAction GetAuthorizedAction()
    {
        if (IsUserAuthorized)
        {
            return PerformSecureAction;
        }
        else
        {
            throw new UnauthorizedAccessException($"Access denied for caller: {_user?.Identity?.Name}");
        }
    }

    private string PerformSecureAction()
    {
        return "Giving permission to action";
    }
}
