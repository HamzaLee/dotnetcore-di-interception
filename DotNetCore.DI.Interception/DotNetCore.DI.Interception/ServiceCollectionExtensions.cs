﻿using System;
using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;

namespace DotNetCore.DI.Interception
{
    public static class ServiceCollectionExtensions
    {
        private static readonly ProxyGenerator Generator = new ProxyGenerator();

        #region Generic

        public static IServiceCollection Add<TService>(
            this IServiceCollection services,
            ServiceLifetime serviceLifetime,
            IInterceptor[] interceptors)
            where TService : class
        {
            TService ImplementationFactory(IServiceProvider serviceProvider) => ActivatorUtilities.CreateInstance<TService>(serviceProvider);

            return Add(services, ImplementationFactory, serviceLifetime, interceptors);
        }

        public static IServiceCollection Add<TService>(
            this IServiceCollection services,
            ServiceLifetime serviceLifetime,
            ProxyGenerationOptions proxyGenerationOptions,
            IInterceptor[] interceptors)
            where TService : class
        {
            TService ImplementationFactory(IServiceProvider serviceProvider) => ActivatorUtilities.CreateInstance<TService>(serviceProvider);

            return Add(services, ImplementationFactory, serviceLifetime, proxyGenerationOptions, interceptors);
        }

        public static IServiceCollection Add<TService, TImplementation>(
            this IServiceCollection services,
            ServiceLifetime serviceLifetime,
            IInterceptor[] interceptors)
           where TService : class
           where TImplementation : class, TService
        {
            TImplementation ImplementationFactory(IServiceProvider serviceProvider) =>
                ActivatorUtilities.GetServiceOrCreateInstance<TImplementation>(serviceProvider);

            return Add<TService, TImplementation>(services, ImplementationFactory, serviceLifetime, interceptors);
        }

        public static IServiceCollection Add<TService, TImplementation>(
            this IServiceCollection services,
            ServiceLifetime serviceLifetime,
            ProxyGenerationOptions proxyGenerationOptions,
            IInterceptor[] interceptors)
            where TService : class
            where TImplementation : class, TService
        {
            TImplementation ImplementationFactory(IServiceProvider serviceProvider) =>
                ActivatorUtilities.GetServiceOrCreateInstance<TImplementation>(serviceProvider);

            return Add<TService, TImplementation>(services, ImplementationFactory, serviceLifetime, proxyGenerationOptions, interceptors);
        }

        public static IServiceCollection Add<TService>(
            this IServiceCollection services,
            Func<IServiceProvider, TService> implementationFactory,
            ServiceLifetime serviceLifetime,
            IInterceptor[] interceptors)
            where TService : class
        {
            if (interceptors == null)
            {
                throw new ArgumentNullException(nameof(interceptors));
            }

            TService ProxyFactory(TService implementationInstance) =>
                Generator.CreateClassProxyWithTarget(implementationInstance, interceptors);

            return Add(services, implementationFactory, serviceLifetime, ProxyFactory);
        }

        public static IServiceCollection Add<TService>(
            this IServiceCollection services,
            Func<IServiceProvider, TService> implementationFactory,
            ServiceLifetime serviceLifetime,
            ProxyGenerationOptions proxyGenerationOptions,
            IInterceptor[] interceptors)
            where TService : class
        {
            if (proxyGenerationOptions == null)
            {
                throw new ArgumentNullException(nameof(proxyGenerationOptions));
            }

            if (interceptors == null)
            {
                throw new ArgumentNullException(nameof(interceptors));
            }

            TService ProxyFactory(TService implementationInstance) =>
                Generator.CreateClassProxyWithTarget(implementationInstance, proxyGenerationOptions, interceptors);

            return Add(services, implementationFactory, serviceLifetime, ProxyFactory);
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
            return Add(services, implementationFactory, ServiceLifetime.Transient, interceptors);
        }

        public static IServiceCollection AddTransient<TService>(
            this IServiceCollection services,
            Func<IServiceProvider, TService> implementationFactory,
            ProxyGenerationOptions proxyGenerationOptions,
            IInterceptor[] interceptors)
            where TService : class
        {
            return Add(services, implementationFactory, ServiceLifetime.Transient, proxyGenerationOptions, interceptors);
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
            return Add(services, implementationFactory, ServiceLifetime.Scoped, interceptors);
        }

        public static IServiceCollection AddScoped<TService>(
            this IServiceCollection services,
            Func<IServiceProvider, TService> implementationFactory,
            ProxyGenerationOptions proxyGenerationOptions,
            IInterceptor[] interceptors)
            where TService : class
        {
            return Add(services, implementationFactory, ServiceLifetime.Scoped, proxyGenerationOptions, interceptors);
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
            return Add(services, implementationFactory, ServiceLifetime.Singleton, interceptors);
        }

        public static IServiceCollection AddSingleton<TService>(
            this IServiceCollection services,
            Func<IServiceProvider, TService> implementationFactory,
            ProxyGenerationOptions proxyGenerationOptions,
            IInterceptor[] interceptors)
            where TService : class
        {
            return Add(services, implementationFactory, ServiceLifetime.Singleton, proxyGenerationOptions, interceptors);
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
