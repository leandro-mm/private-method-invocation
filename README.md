# Considerations about C# access control on private methods
> code examples using dotnet and C#

## The Context
You have a class with a private method and you want to make invocation of the method possible by certain callers.

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


## Situation 2: Make the private method internal and use InternalsVisibleTo attribute
### Option 1: Create a Wrapper with Action Type

### Option 2: Add a public wrapper method that includes authorization logic

