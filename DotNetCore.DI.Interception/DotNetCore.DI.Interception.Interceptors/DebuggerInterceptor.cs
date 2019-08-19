using System.Diagnostics;

namespace DotNetCore.DI.Interception.Interceptors
{
    public class DebuggerInterceptor : WriterInterceptor
    {
        public DebuggerInterceptor() : base(text => Debug.WriteLine(text))
        {
        }
    }
}
