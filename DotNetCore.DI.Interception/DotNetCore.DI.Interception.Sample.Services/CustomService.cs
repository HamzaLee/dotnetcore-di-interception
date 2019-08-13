namespace DotNetCore.DI.Interception.Sample.Services
{
    public class CustomService : ICustomService
    {
        public string GetValue() => "Hello World";
    }
}