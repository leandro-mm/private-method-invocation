# Considerations about C# access control on private methods
> code examples using dotnet and C#

## The Context
You have a class with a private method and you want to make invocation of the method possible by certain callers.

 ```csharp 
public class SecureServiceClass
{ 
    // Private method we want to protect
    private void PerformSecureAction()
    {
        Console.WriteLine("Sensitive operation executed with success.");
    }
}
```

## Situation 1: you truly cannot modify the original class
As your method is private, you cannot directly reference it from outside the class. However, you have a few options:

###  Option 1: Use Reflection
 ```csharp 
public class DelegateService
{
    public Action GetSecureActionDelegate(FormSecureService form, string callerId)
    {
        if (!IsAuthorized(callerId))
            throw new UnauthorizedAccessException($"Access denied for caller: {callerId}");
        
        // Use reflection to get the private method
        var method = typeof(FormSecureService).GetMethod("PerformSecureAction", 
                     System.Reflection.BindingFlags.NonPublic | 
                     System.Reflection.BindingFlags.Instance);
        
        if (method == null)
            throw new InvalidOperationException("Method not found");
        
        // Create and return delegate
        return (Action)Delegate.CreateDelegate(typeof(Action), form, method);
    }
    
    private bool IsAuthorized(string callerId)
    {
        var authorizedCallers = new HashSet<string> { "Admin", "TrustedService" };
        return authorizedCallers.Contains(callerId);
    }
}
```
### Option 2: Use Dynamic Invocation
 ```csharp 
public class DynamicDelegateService
{
    public dynamic GetSecureActionDelegate(FormSecureService form, string callerId)
    {
        if (!IsAuthorized(callerId))
            throw new UnauthorizedAccessException($"Access denied for caller: {callerId}");
        
        return new DynamicMethodInvoker(form, "PerformSecureAction");
    }
    
    private class DynamicMethodInvoker
    {
        private readonly object _target;
        private readonly string _methodName;
        
        public DynamicMethodInvoker(object target, string methodName)
        {
            _target = target;
            _methodName = methodName;
        }
        
        public void Invoke()
        {
            var method = _target.GetType().GetMethod(_methodName,
                         System.Reflection.BindingFlags.NonPublic | 
                         System.Reflection.BindingFlags.Instance);
            method?.Invoke(_target, null);
        }
    }
}
```
### Option 3: Create a Wrapper with Action Type
 ```csharp 
public class SecureDelegateService
{
    public Action GetAuthorizedDelegate(FormSecureService form, string callerId)
    {
        if (!IsAuthorized(callerId))
            throw new UnauthorizedAccessException($"Access denied for caller: {callerId}");
        
        // Return an action that uses reflection internally
        return () =>
        {
            var method = typeof(FormSecureService).GetMethod("PerformSecureAction",
                         System.Reflection.BindingFlags.NonPublic | 
                         System.Reflection.BindingFlags.Instance);
            method?.Invoke(form, null);
        };
    }
    
    private bool IsAuthorized(string callerId)
    {
        var authorizedCallers = new HashSet<string> { "Admin", "TrustedService" };
        return authorizedCallers.Contains(callerId);
    }
}
```

#### Important Considerations 
| Caractesristic | Consideration |
| -------- | ------- | 
| Reflection Performance | Reflection is slower than direct method calls | 
| Maintainability | If the private method name changes, the service will break silently | 
| Security | Reflection bypasses C# access modifiers - ensure your authorization logic is robust | 
| Trust Boundary | This approach is valid for internal tooling but not recommended for security-critical systems | 

## Situation 2: You can modify the original class
Considering you can modify the original class, you might want to add a public wrapper method 
that includes authorization logic to make invocation of the method by certain callers.

- Start by defining a delegate type that represents the method signature you want to expose
 ```csharp 
public class SecureServiceClass
{
    public delegate void AuthorizedAction();

    // Private method we want to protect
    private void PerformSecureAction()
    {
        Console.WriteLine("Sensitive operation executed with success.");
    }
}
```
- Step number 2: define the wrapper to validate authorization and return the AuthorizedAction delegate
 ```csharp 
    public AuthorizedAction GetAuthorizedDelegate(string callerId)
    {
        if (IsAuthorized(callerId))
        {
            return PerformSecureAction;
        }
        else
        {
            throw new UnauthorizedAccessException($"Access denied for caller: {callerId}");
        }
    }

    private bool IsAuthorized(string callerId)
    {
        // Simulated authorization logic
        var authorizedCallers = new HashSet<string> { "Admin", "TrustedService" };
        return authorizedCallers.Contains(callerId);
    }
```
#### Benefits of This Approach 
| Caractesristic | Consideration |
| -------- | ------- | 
| No Reflection Required | Clean, type-safe, and performant | 
| Encapsulation Maintained | The private method remains private | 
| Clear Authorization | Reflection bypasses C# access modifiers - ensure your authorization logic is robust | 
| Testable | Easy to unit test the authorization logic | 
| Discoverable | Other developers can see the public methods via IntelliSense | 

## Running the Example
- [Download .NET](https://dotnet.microsoft.com/en-us/download)
- [Download Visual Studio](https://visualstudio.microsoft.com/pt-br/downloads/)
- Clone the repository and open the solution in Visual Studio
- Build and run the application
- Observe how changes in the main form update all open views
- Plugins load automatically and respond to data changes
