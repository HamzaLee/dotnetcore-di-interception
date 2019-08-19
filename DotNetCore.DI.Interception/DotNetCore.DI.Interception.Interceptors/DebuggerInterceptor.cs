using System.Diagnostics;
using Castle.DynamicProxy;

namespace DotNetCore.DI.Interception.Interceptors
{
    public class DebuggerInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {

            Debug.WriteLine($"-- Before {invocation.Method.Name} in {invocation.TargetType}");
            invocation.Proceed();
            Debug.WriteLine($"-- After {invocation.Method.Name} in {invocation.TargetType}");
        }
    }
}
