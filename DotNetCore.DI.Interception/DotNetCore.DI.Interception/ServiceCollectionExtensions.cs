using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DotNetCore.DI.Interception
{
    public static class ServiceCollectionExtensions
    {
        private static readonly ProxyGenerator Generator = new ProxyGenerator();

        #region AddTransient

        public static IServiceCollection AddTransient<TService>(this IServiceCollection services, IInterceptor[] interceptors)
            where TService : class
        {
            return services.AddTransient(typeof(TService), provider => Generator.CreateClassProxy<TService>(interceptors));
        }
        public static IServiceCollection AddTransient<TService, TImplementation>(this IServiceCollection services, IInterceptor[] interceptors)
            where TService : class
            where TImplementation : class, TService
        {
            return services.AddTransient(typeof(TService), provider => Generator.CreateClassProxy<TImplementation>(interceptors));
        }
        public static IServiceCollection AddTransient<TService>(this IServiceCollection services,
            Func<IServiceProvider, TService> implementationFactory, IInterceptor[] interceptors)
            where TService : class
        {
            return services.AddTransient(typeof(TService), provider => Generator.CreateClassProxyWithTarget(implementationFactory(provider), interceptors));
        }
        public static IServiceCollection AddTransient<TService, TImplementation>(this IServiceCollection services,
            Func<IServiceProvider, TImplementation> implementationFactory, IInterceptor[] interceptors)
            where TService : class
            where TImplementation : class, TService
        {
            return services.AddTransient(typeof(TService), provider => Generator.CreateClassProxyWithTarget(implementationFactory(provider), interceptors));
        }

        #endregion

        #region AddScoped

        public static IServiceCollection AddScoped<TService>(this IServiceCollection services, IInterceptor[] interceptors)
            where TService : class
        {
            return services.AddScoped(typeof(TService), provider => Generator.CreateClassProxy<TService>(interceptors));
        }
        public static IServiceCollection AddScoped<TService, TImplementation>(this IServiceCollection services, IInterceptor[] interceptors)
            where TService : class
            where TImplementation : class, TService
        {
            return services.AddScoped(typeof(TService), provider => Generator.CreateClassProxy<TImplementation>(interceptors));
        }
        public static IServiceCollection AddScoped<TService>(this IServiceCollection services,
            Func<IServiceProvider, TService> implementationFactory, IInterceptor[] interceptors)
            where TService : class
        {
            return services.AddScoped(typeof(TService), provider => Generator.CreateClassProxyWithTarget(implementationFactory(provider), interceptors));
        }
        public static IServiceCollection AddScoped<TService, TImplementation>(this IServiceCollection services,
            Func<IServiceProvider, TImplementation> implementationFactory, IInterceptor[] interceptors)
            where TService : class
            where TImplementation : class, TService
        {
            return services.AddScoped(typeof(TService), provider => Generator.CreateClassProxyWithTarget(implementationFactory(provider), interceptors));
        }

        #endregion

        #region AddSingleton

        public static IServiceCollection AddSingleton<TService>(this IServiceCollection services, IInterceptor[] interceptors)
            where TService : class
        {
            return services.AddSingleton(typeof(TService), provider => Generator.CreateClassProxy<TService>(interceptors));
        }
        public static IServiceCollection AddSingleton<TService, TImplementation>(this IServiceCollection services, IInterceptor[] interceptors)
            where TService : class
            where TImplementation : class, TService
        {
            return services.AddSingleton(typeof(TService), provider => Generator.CreateClassProxy<TImplementation>(interceptors));
        }
        public static IServiceCollection AddSingleton<TService>(this IServiceCollection services,
            Func<IServiceProvider, TService> implementationFactory, IInterceptor[] interceptors)
            where TService : class
        {
            return services.AddSingleton(typeof(TService), provider => Generator.CreateClassProxyWithTarget(implementationFactory(provider), interceptors));
        }
        public static IServiceCollection AddSingleton<TService, TImplementation>(this IServiceCollection services,
            Func<IServiceProvider, TImplementation> implementationFactory, IInterceptor[] interceptors)
            where TService : class
            where TImplementation : class, TService
        {
            return services.AddSingleton(typeof(TService), provider => Generator.CreateClassProxyWithTarget(implementationFactory(provider), interceptors));
        }

        #endregion
    }
}
