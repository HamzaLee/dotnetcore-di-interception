namespace DotNetCore.DI.Interception.Sample.Services
{
    public class CustomService : ICustomService
    {
        public virtual string GetValue() => "Hello World";
    }
}
