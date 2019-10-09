using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.Linq;

namespace DotNetCore.DI.Interception
{
    public static class ServiceCollectionExtensions
    {
        private static readonly ProxyGenerator Generator = new ProxyGenerator();

        #region Generic
        // BUG
        public static IServiceCollection Add<TService>(
            this IServiceCollection services,
            ServiceLifetime serviceLifetime,
            IInterceptor[] interceptors)
            where TService : class
        {
            return Add<TService, TService>(services, serviceLifetime, interceptors);
        }

        // BUG
        public static IServiceCollection Add<TService>(
            this IServiceCollection services,
            ServiceLifetime serviceLifetime,
            ProxyGenerationOptions proxyGenerationOptions,
            IInterceptor[] interceptors)
            where TService : class
        {
            return Add<TService, TService>(services, serviceLifetime, proxyGenerationOptions, interceptors);
        }

        public static IServiceCollection Add<TService, TImplementation>(
            this IServiceCollection services,
            ServiceLifetime serviceLifetime,
            IInterceptor[] interceptors)
           where TService : class
           where TImplementation : class, TService
        {
            return Add<TService, TImplementation>(services, ActivatorUtilities.GetServiceOrCreateInstance<TImplementation>, serviceLifetime, interceptors);
        }

        public static IServiceCollection Add<TService, TImplementation>(
            this IServiceCollection services,
            ServiceLifetime serviceLifetime,
            ProxyGenerationOptions proxyGenerationOptions,
            IInterceptor[] interceptors)
            where TService : class
            where TImplementation : class, TService
        {
            return Add<TService, TImplementation>(services, ActivatorUtilities.GetServiceOrCreateInstance<TImplementation>, serviceLifetime, proxyGenerationOptions, interceptors);
        }

        // BUG : Specified type is not an interface Parameter name: interfaceToProxy
        public static IServiceCollection Add<TService>(
            this IServiceCollection services,
            Func<IServiceProvider, TService> implementationFactory,
            ServiceLifetime serviceLifetime,
            IInterceptor[] interceptors)
            where TService : class
        {
            return Add<TService, TService>(services, implementationFactory, serviceLifetime, interceptors);
        }

        // BUG : Specified type is not an interface Parameter name: interfaceToProxy
        public static IServiceCollection Add<TService>(
            this IServiceCollection services,
            Func<IServiceProvider, TService> implementationFactory,
            ServiceLifetime serviceLifetime,
            ProxyGenerationOptions proxyGenerationOptions,
            IInterceptor[] interceptors)
            where TService : class
        {
            return Add<TService, TService>(services, implementationFactory, serviceLifetime, proxyGenerationOptions, interceptors);
        }

        public static IServiceCollection Add<TService, TImplementation>(
            this IServiceCollection services,
            Func<IServiceProvider, TImplementation> implementationFactory,
            ServiceLifetime serviceLifetime,
            IInterceptor[] interceptors)
            where TService : class
            where TImplementation : class, TService
        {
            if (interceptors == null)
            {
                throw new ArgumentNullException(nameof(interceptors));
            }

            TService ProxyFactory(TImplementation implementationInstance) =>
                Generator.CreateInterfaceProxyWithTarget<TService>(implementationInstance, interceptors);

            return Add(services, implementationFactory, serviceLifetime, ProxyFactory);
        }

        public static IServiceCollection Add<TService, TImplementation>(
            this IServiceCollection services,
            Func<IServiceProvider, TImplementation> implementationFactory,
            ServiceLifetime serviceLifetime,
            ProxyGenerationOptions proxyGenerationOptions,
            IInterceptor[] interceptors)
            where TService : class
            where TImplementation : class, TService
        {
            if (proxyGenerationOptions == null)
            {
                throw new ArgumentNullException(nameof(proxyGenerationOptions));
            }

            if (interceptors == null)
            {
                throw new ArgumentNullException(nameof(interceptors));
            }

            TService ProxyFactory(TImplementation implementationInstance) =>
                Generator.CreateInterfaceProxyWithTarget<TService>(implementationInstance, proxyGenerationOptions, interceptors);

            return Add(services, implementationFactory, serviceLifetime, ProxyFactory);
        }

        public static IServiceCollection Add<TService, TImplementation>(
            this IServiceCollection services,
            Func<IServiceProvider, TImplementation> implementationFactory,
            ServiceLifetime serviceLifetime,
            Func<TImplementation, TService> proxyFactory)
            where TService : class
            where TImplementation : class, TService
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (implementationFactory == null)
            {
                throw new ArgumentNullException(nameof(implementationFactory));
            }

            if (proxyFactory == null)
            {
                throw new ArgumentNullException(nameof(proxyFactory));
            }

            var serviceDescriptor = new ServiceDescriptor(
                typeof(TService),
                provider =>
                {
                    var implementationInstance = implementationFactory(provider);
                    return proxyFactory(implementationInstance);
                },
                serviceLifetime);

            services.Add(serviceDescriptor);
            return services;
        }

        #endregion

        #region AddTransient

        public static IServiceCollection AddTransient<TService>(this IServiceCollection services, IInterceptor[] interceptors)
         where TService : class
        {
            return Add<TService>(services, ServiceLifetime.Transient, interceptors);
        }

        public static IServiceCollection AddTransient<TService>(this IServiceCollection services, ProxyGenerationOptions proxyGenerationOptions, IInterceptor[] interceptors)
            where TService : class
        {
            return Add<TService>(services, ServiceLifetime.Transient, proxyGenerationOptions, interceptors);
        }

        public static IServiceCollection AddTransient<TService, TImplementation>(this IServiceCollection services, IInterceptor[] interceptors)
            where TService : class
            where TImplementation : class, TService
        {
            return Add<TService, TImplementation>(services, ServiceLifetime.Transient, interceptors);
        }

        public static IServiceCollection AddTransient<TService, TImplementation>(
            this IServiceCollection services,
            ProxyGenerationOptions proxyGenerationOptions,
            IInterceptor[] interceptors)
            where TService : class
            where TImplementation : class, TService
        {
            return Add<TService, TImplementation>(services, ServiceLifetime.Transient, proxyGenerationOptions, interceptors);
        }

        public static IServiceCollection AddTransient<TService>(
            this IServiceCollection services,
            Func<IServiceProvider, TService> implementationFactory,
            IInterceptor[] interceptors)
            where TService : class
        {
            return Add<TService>(services, implementationFactory, ServiceLifetime.Transient, interceptors);
        }

        public static IServiceCollection AddTransient<TService>(
            this IServiceCollection services,
            Func<IServiceProvider, TService> implementationFactory,
            ProxyGenerationOptions proxyGenerationOptions,
            IInterceptor[] interceptors)
            where TService : class
        {
            return Add<TService>(services, implementationFactory, ServiceLifetime.Transient, proxyGenerationOptions, interceptors);
        }

        public static IServiceCollection AddTransient<TService, TImplementation>(
            this IServiceCollection services,
            Func<IServiceProvider, TImplementation> implementationFactory,
            IInterceptor[] interceptors)
            where TService : class
            where TImplementation : class, TService
        {
            return Add<TService, TImplementation>(services, implementationFactory, ServiceLifetime.Transient, interceptors);
        }

        public static IServiceCollection AddTransient<TService, TImplementation>(
            this IServiceCollection services,
            Func<IServiceProvider, TImplementation> implementationFactory,
            ProxyGenerationOptions proxyGenerationOptions,
            IInterceptor[] interceptors)
            where TService : class
            where TImplementation : class, TService
        {
            return Add<TService, TImplementation>(services, implementationFactory, ServiceLifetime.Transient, proxyGenerationOptions, interceptors);
        }

        #endregion

        #region AddScoped

        public static IServiceCollection AddScoped<TService>(this IServiceCollection services, IInterceptor[] interceptors)
            where TService : class
        {
            return Add<TService>(services, ServiceLifetime.Scoped, interceptors);
        }

        public static IServiceCollection AddScoped<TService>(this IServiceCollection services, ProxyGenerationOptions proxyGenerationOptions, IInterceptor[] interceptors)
            where TService : class
        {
            return Add<TService>(services, ServiceLifetime.Scoped, proxyGenerationOptions, interceptors);
        }

        public static IServiceCollection AddScoped<TService, TImplementation>(this IServiceCollection services, IInterceptor[] interceptors)
            where TService : class
            where TImplementation : class, TService
        {
            return Add<TService, TImplementation>(services, ServiceLifetime.Scoped, interceptors);
        }

        public static IServiceCollection AddScoped<TService, TImplementation>(this IServiceCollection services, ProxyGenerationOptions proxyGenerationOptions, IInterceptor[] interceptors)
            where TService : class
            where TImplementation : class, TService
        {
            return Add<TService, TImplementation>(services, ServiceLifetime.Scoped, proxyGenerationOptions, interceptors);
        }

        public static IServiceCollection AddScoped<TService>(
            this IServiceCollection services,
            Func<IServiceProvider, TService> implementationFactory,
            IInterceptor[] interceptors)
            where TService : class
        {
            return Add<TService>(services, implementationFactory, ServiceLifetime.Scoped, interceptors);
        }

        public static IServiceCollection AddScoped<TService>(
            this IServiceCollection services,
            Func<IServiceProvider, TService> implementationFactory,
            ProxyGenerationOptions proxyGenerationOptions,
            IInterceptor[] interceptors)
            where TService : class
        {
            return Add<TService>(services, implementationFactory, ServiceLifetime.Scoped, proxyGenerationOptions, interceptors);
        }

        public static IServiceCollection AddScoped<TService, TImplementation>(
            this IServiceCollection services,
            Func<IServiceProvider, TImplementation> implementationFactory,
            IInterceptor[] interceptors)
            where TService : class
            where TImplementation : class, TService
        {
            return Add<TService, TImplementation>(services, implementationFactory, ServiceLifetime.Scoped, interceptors);
        }

        public static IServiceCollection AddScoped<TService, TImplementation>(
            this IServiceCollection services,
            Func<IServiceProvider, TImplementation> implementationFactory,
            ProxyGenerationOptions proxyGenerationOptions,
            IInterceptor[] interceptors)
            where TService : class
            where TImplementation : class, TService
        {
            return Add<TService, TImplementation>(services, implementationFactory, ServiceLifetime.Scoped, proxyGenerationOptions, interceptors);
        }

        #endregion

        #region AddSingleton

        public static IServiceCollection AddSingleton<TService>(this IServiceCollection services, IInterceptor[] interceptors)
            where TService : class
        {
            return Add<TService>(services, ServiceLifetime.Singleton, interceptors);
        }

        public static IServiceCollection AddSingleton<TService>(this IServiceCollection services, ProxyGenerationOptions proxyGenerationOptions, IInterceptor[] interceptors)
            where TService : class
        {
            return Add<TService>(services, ServiceLifetime.Singleton, proxyGenerationOptions, interceptors);
        }

        public static IServiceCollection AddSingleton<TService, TImplementation>(this IServiceCollection services, IInterceptor[] interceptors)
            where TService : class
            where TImplementation : class, TService
        {
            return Add<TService, TImplementation>(services, ServiceLifetime.Singleton, interceptors);
        }

        public static IServiceCollection AddSingleton<TService, TImplementation>(this IServiceCollection services, ProxyGenerationOptions proxyGenerationOptions, IInterceptor[] interceptors)
            where TService : class
            where TImplementation : class, TService
        {
            return Add<TService, TImplementation>(services, ServiceLifetime.Singleton, proxyGenerationOptions, interceptors);
        }

        public static IServiceCollection AddSingleton<TService>(
            this IServiceCollection services,
            Func<IServiceProvider, TService> implementationFactory,
            IInterceptor[] interceptors)
            where TService : class
        {
            return Add<TService>(services, implementationFactory, ServiceLifetime.Singleton, interceptors);
        }

        public static IServiceCollection AddSingleton<TService>(
            this IServiceCollection services,
            Func<IServiceProvider, TService> implementationFactory,
            ProxyGenerationOptions proxyGenerationOptions,
            IInterceptor[] interceptors)
            where TService : class
        {
            return Add<TService>(services, implementationFactory, ServiceLifetime.Singleton, proxyGenerationOptions, interceptors);
        }

        public static IServiceCollection AddSingleton<TService, TImplementation>(
            this IServiceCollection services,
            Func<IServiceProvider, TImplementation> implementationFactory,
            IInterceptor[] interceptors)
            where TService : class
            where TImplementation : class, TService
        {
            return Add<TService, TImplementation>(services, implementationFactory, ServiceLifetime.Singleton, interceptors);
        }

        public static IServiceCollection AddSingleton<TService, TImplementation>(
            this IServiceCollection services,
            Func<IServiceProvider, TImplementation> implementationFactory,
            ProxyGenerationOptions proxyGenerationOptions,
            IInterceptor[] interceptors)
            where TService : class
            where TImplementation : class, TService
        {
            return Add<TService, TImplementation>(services, implementationFactory, ServiceLifetime.Singleton, proxyGenerationOptions, interceptors);
        }

        #endregion
    }
}
