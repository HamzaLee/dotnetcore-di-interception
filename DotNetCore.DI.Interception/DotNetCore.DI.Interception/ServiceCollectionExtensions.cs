using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace DotNetCore.DI.Interception
{
    public static class ServiceCollectionExtensions
    {
        private static readonly ProxyGenerator Generator = new ProxyGenerator();

        private static IServiceCollection AddResolvedService<TService, TImplementation>(this IServiceCollection services,
            ServiceLifetime serviceLifetime,
            IInterceptor[] interceptors)
            where TService : class
            where TImplementation : class, TService
        {
            var serviceDescriptor = new ServiceDescriptor(typeof(TService), provider =>
            {

                var implementationInstance = provider.GetRequiredService<TImplementation>();
                var sd = services.First(s => s.ServiceType == typeof(TImplementation));
                services.Remove(sd);
                return Generator.CreateInterfaceProxyWithTarget<TService>(implementationInstance, interceptors);
            }, serviceLifetime);
            services.Add(serviceDescriptor);
            return services;
        }

        #region Generic

        public static IServiceCollection Add<TService>(this IServiceCollection services,
            ServiceLifetime serviceLifetime,
            IInterceptor[] interceptors)
            where TService : class
        {
            return Add<TService, TService>(services, serviceLifetime, interceptors);
        }
        public static IServiceCollection Add<TService, TImplementation>(this IServiceCollection services,
           ServiceLifetime serviceLifetime,
           IInterceptor[] interceptors)
           where TService : class
           where TImplementation : class, TService
        {
            services.Add(new ServiceDescriptor(typeof(TImplementation), typeof(TImplementation), serviceLifetime));
            return AddResolvedService<TService, TImplementation>(services, serviceLifetime, interceptors);
        }
        public static IServiceCollection Add<TService>(this IServiceCollection services,
            Func<IServiceProvider, TService> implementationFactory, ServiceLifetime serviceLifetime,
            IInterceptor[] interceptors)
            where TService : class
        {
            return Add<TService, TService>(services, implementationFactory, serviceLifetime, interceptors);
        }
        public static IServiceCollection Add<TService, TImplementation>(this IServiceCollection services,
            Func<IServiceProvider, TService> implementationFactory, ServiceLifetime serviceLifetime,
            IInterceptor[] interceptors)
            where TService : class
            where TImplementation : class, TService
        {
            services.Add(new ServiceDescriptor(typeof(TImplementation), implementationFactory, serviceLifetime));
            return AddResolvedService<TService, TImplementation>(services, serviceLifetime, interceptors);
        }
        
        #endregion

        #region AddTransient

        public static IServiceCollection AddTransient<TService>(this IServiceCollection services, IInterceptor[] interceptors)
            where TService : class
        {
            return Add<TService>(services, ServiceLifetime.Transient, interceptors);
        }
        public static IServiceCollection AddTransient<TService, TImplementation>(this IServiceCollection services, IInterceptor[] interceptors)
            where TService : class
            where TImplementation : class, TService
        {
            return Add<TService, TImplementation>(services, ServiceLifetime.Transient, interceptors);
        }
        public static IServiceCollection AddTransient<TService>(this IServiceCollection services,
            Func<IServiceProvider, TService> implementationFactory, IInterceptor[] interceptors)
            where TService : class
        {
            return Add<TService>(services, implementationFactory, ServiceLifetime.Transient, interceptors);
        }
        public static IServiceCollection AddTransient<TService, TImplementation>(this IServiceCollection services,
            Func<IServiceProvider, TImplementation> implementationFactory, IInterceptor[] interceptors)
            where TService : class
            where TImplementation : class, TService
        {
            return Add<TService, TImplementation>(services, implementationFactory, ServiceLifetime.Transient, interceptors);
        }

        #endregion

        #region AddScoped

        public static IServiceCollection AddScoped<TService>(this IServiceCollection services, IInterceptor[] interceptors)
            where TService : class
        {
            return Add<TService>(services, ServiceLifetime.Scoped, interceptors);
        }
        public static IServiceCollection AddScoped<TService, TImplementation>(this IServiceCollection services, IInterceptor[] interceptors)
            where TService : class
            where TImplementation : class, TService
        {
            return Add<TService, TImplementation>(services, ServiceLifetime.Scoped, interceptors);
        }
        public static IServiceCollection AddScoped<TService>(this IServiceCollection services,
            Func<IServiceProvider, TService> implementationFactory, IInterceptor[] interceptors)
            where TService : class
        {
            return Add<TService>(services, implementationFactory, ServiceLifetime.Scoped, interceptors);
        }
        public static IServiceCollection AddScoped<TService, TImplementation>(this IServiceCollection services,
            Func<IServiceProvider, TImplementation> implementationFactory, IInterceptor[] interceptors)
            where TService : class
            where TImplementation : class, TService
        {
            return Add<TService, TImplementation>(services, implementationFactory, ServiceLifetime.Scoped, interceptors);
        }

        #endregion

        #region AddSingleton

        public static IServiceCollection AddSingleton<TService>(this IServiceCollection services, IInterceptor[] interceptors)
            where TService : class
        {
            return Add<TService>(services, ServiceLifetime.Singleton, interceptors);
        }
        public static IServiceCollection AddSingleton<TService, TImplementation>(this IServiceCollection services, IInterceptor[] interceptors)
            where TService : class
            where TImplementation : class, TService
        {
            return Add<TService, TImplementation>(services, ServiceLifetime.Singleton, interceptors);
        }
        public static IServiceCollection AddSingleton<TService>(this IServiceCollection services,
            Func<IServiceProvider, TService> implementationFactory, IInterceptor[] interceptors)
            where TService : class
        {
            return Add<TService>(services, implementationFactory, ServiceLifetime.Singleton, interceptors);
        }
        public static IServiceCollection AddSingleton<TService, TImplementation>(this IServiceCollection services,
            Func<IServiceProvider, TImplementation> implementationFactory, IInterceptor[] interceptors)
            where TService : class
            where TImplementation : class, TService
        {
            return Add<TService, TImplementation>(services, implementationFactory, ServiceLifetime.Singleton, interceptors);
        }

        #endregion
    }
}
