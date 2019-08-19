using System;
using Castle.DynamicProxy;

namespace DotNetCore.DI.Interception.Interceptors
{
    public abstract class TryCatchWriterInterceptor : TryCatchWrapperInterceptor
    {
        protected TryCatchWriterInterceptor(bool throwException = false) : base(throwException)
        {
        }
        public abstract void Writer(string message);
        public abstract Func<IInvocation, string> OnSuccessMessage { get; }
        public abstract Func<IInvocation, Exception, string> OnFailureMessage { get; }
        public override Action<IInvocation> OnSuccess => invocation => Writer(OnSuccessMessage(invocation));
        public override Action<IInvocation, Exception> OnFailure =>
            (invocation, exception) => Writer(OnFailureMessage(invocation, exception));
    }
}