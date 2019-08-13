using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;

namespace DotNetCore.DI.Interception
{
    public static class ServiceCollectionExtensions
    {
        private static readonly ProxyGenerator Generator = new ProxyGenerator();

        public static IServiceCollection AddTransientX<TService, TImplementation>(this IServiceCollection services, params IInterceptor[] interceptors)
            where TService : class
            where TImplementation : class, TService
        {
            return services.AddTransient<TService, TImplementation>(provider => Generator.CreateClassProxy<TImplementation>(interceptors));
        }

    }
}
