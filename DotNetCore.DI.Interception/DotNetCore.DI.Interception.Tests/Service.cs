namespace DotNetCore.DI.Interception.Tests
{
    public interface ITestService
    {
    }

    public class TestService : ITestService
    {
    }

    public class TestProxyService : ITestService
    {
        public TestProxyService(ITestService service)
        {
            InnerService = service;
        }

        public ITestService InnerService { get; }
    }
}