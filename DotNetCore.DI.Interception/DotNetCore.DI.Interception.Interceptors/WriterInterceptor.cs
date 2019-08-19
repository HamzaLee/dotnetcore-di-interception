using System;
using Castle.DynamicProxy;

namespace DotNetCore.DI.Interception.Interceptors
{
    public class WriterInterceptor : IInterceptor
    {
        private readonly Action<string> _writer;

        public WriterInterceptor(Action<string> writer)
        {
            _writer = writer;
        }
        public void Intercept(IInvocation invocation)
        {

            _writer($"-- Before {invocation.Method.Name} in {invocation.TargetType}");

            invocation.Proceed();
            _writer($"-- After {invocation.Method.Name} in {invocation.TargetType}");
        }
    }
}