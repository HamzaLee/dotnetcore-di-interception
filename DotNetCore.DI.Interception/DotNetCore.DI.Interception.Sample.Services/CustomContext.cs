namespace DotNetCore.DI.Interception.Sample.Services
{
    public class CustomContext : ICustomContext
    {
        public string GetValue()
        {
            return nameof(CustomContext);
        }
    }
}