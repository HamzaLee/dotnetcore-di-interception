using System;
using Castle.DynamicProxy;

namespace DotNetCore.DI.Interception.Interceptors
{
    public abstract class TryCatchWrapperInterceptor : IInterceptor
    {
        private readonly bool _throwException;

        protected TryCatchWrapperInterceptor(bool throwException)
        {
            _throwException = throwException;
        }
        public void Intercept(IInvocation invocation)
        {
            try
            {
                invocation.Proceed();
                OnSuccess?.Invoke(invocation);
            }
            catch (Exception ex)
            {
                OnFailure?.Invoke(invocation, ex);
                if (_throwException)
                {
                    throw;
                }
            }

        }
        public abstract Action<IInvocation> OnSuccess { get; }
        public abstract Action<IInvocation, Exception> OnFailure { get; }
    }
}