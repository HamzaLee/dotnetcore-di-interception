using System;
using System.Diagnostics;
using Castle.DynamicProxy;

namespace DotNetCore.DI.Interception.Interceptors
{
    public class DebuggerTryCatchWriterInterceptor : TryCatchWriterInterceptor
    {
        public override void Writer(string message) => Debug.WriteLine(message);
        public override Func<IInvocation, string> OnSuccessMessage => invocation => $"Success {invocation.Method.Name}";
        public override Func<IInvocation, Exception, string> OnFailureMessage => (invocation, exception) => $"Failed {invocation.Method.Name} {exception.Message}";

    }
}