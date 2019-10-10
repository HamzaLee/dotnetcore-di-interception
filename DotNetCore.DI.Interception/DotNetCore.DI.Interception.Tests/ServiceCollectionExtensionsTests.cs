using System;
using System.Diagnostics.CodeAnalysis;
using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace DotNetCore.DI.Interception.Tests
{
    [SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global")]
    public class ServiceCollectionExtensionsTests
    {
        #region Add

        [Test]
        public void Add()
        {
            var services = new ServiceCollection();
            TestService ImplementationFactory(IServiceProvider sp) => new TestService();
            ITestService ProxyFactory(TestService s) => new TestProxyService(s);

            services.Add(ImplementationFactory, ServiceLifetime.Transient, ProxyFactory);

            var serviceProvider = services.BuildServiceProvider();
            var service = (TestProxyService)serviceProvider.GetService<ITestService>();

            Assert.IsNotNull(service);
            Assert.IsNotNull(service.InnerService);
            Assert.IsInstanceOf<TestService>(service.InnerService);
        }

        [Test]
        public void Add_WhenServicesIsNull_ThenThrowArgumentNullException()
        {
            TestService ImplementationFactory(IServiceProvider sp) => new TestService();
            ITestService ProxyFactory(TestService s) => new TestProxyService(s);

            Assert.Throws<ArgumentNullException>(() =>
                ServiceCollectionExtensions.Add(
                  null,
                  ImplementationFactory,
                  ServiceLifetime.Transient,
                  ProxyFactory));
        }

        [Test]
        public void Add_WhenImplementationFactoryIsNull_ThenThrowArgumentNullException()
        {
            var services = new ServiceCollection();
            ITestService ProxyFactory(TestService s) => new TestProxyService(s);

            Assert.Throws<ArgumentNullException>(() =>
                services.Add<ITestService, TestService>(
                    null,
                    ServiceLifetime.Transient,
                    ProxyFactory));
        }

        [Test]
        public void Add_WhenProxyFactoryIsNull_ThenThrowArgumentNullException()
        {
            var services = new ServiceCollection();
            TestService ImplementationFactory(IServiceProvider sp) => new TestService();

            Assert.Throws<ArgumentNullException>(() =>
                services.Add<ITestService, TestService>(
                    ImplementationFactory,
                    ServiceLifetime.Transient,
                    proxyFactory: null));
        }

        #endregion

        #region Interface resolution

        #region AddDynamicProxyWithImplementationFactoryAndOptionsForInterfaceResolution

        [Test]
        public void AddDynamicProxyWithImplementationFactoryAndOptionsForInterfaceResolution()
        {
            var services = new ServiceCollection();
            TestService ImplementationFactory(IServiceProvider sp) => new TestService();
            var proxyGenerationOptions = new ProxyGenerationOptions();
            IInterceptor[] interceptors = { new StandardInterceptor() };

            services.Add<ITestService, TestService>(
                ImplementationFactory,
                ServiceLifetime.Transient,
                proxyGenerationOptions,
                interceptors);

            var serviceProvider = services.BuildServiceProvider();
            var service = serviceProvider.GetService<ITestService>();

            Assert.IsNotNull(service);
            Assert.IsInstanceOf<ITestService>(service);

            var proxyTargetAccessor = service as IProxyTargetAccessor;
            Assert.IsNotNull(proxyTargetAccessor);

            var actualInterceptors = proxyTargetAccessor.GetInterceptors();
            Assert.AreEqual(actualInterceptors.Length, interceptors.Length);
            Assert.AreEqual(actualInterceptors[0], interceptors[0]);

            var proxyTarget = proxyTargetAccessor.DynProxyGetTarget();
            Assert.IsInstanceOf<TestService>(proxyTarget);
        }

        [Test]
        public void AddDynamicProxyWithImplementationFactoryAndOptionsForInterfaceResolution_WhenProxyGenerationOptionsIsNull_ThenThrowException()
        {
            var services = new ServiceCollection();
            TestService ImplementationFactory(IServiceProvider sp) => new TestService();
            IInterceptor[] interceptors = { new StandardInterceptor() };

            Assert.Throws<ArgumentNullException>(() => services.Add<ITestService, TestService>(
                ImplementationFactory,
                ServiceLifetime.Transient,
                null,
                interceptors));
        }

        [Test]
        public void AddDynamicProxyWithImplementationFactoryAndOptionsForInterfaceResolution_WhenInterceptorsIsNull_ThenThrowException()
        {
            var services = new ServiceCollection();
            TestService ImplementationFactory(IServiceProvider sp) => new TestService();
            var proxyGenerationOptions = new ProxyGenerationOptions();

            Assert.Throws<ArgumentNullException>(() => services.Add<ITestService, TestService>(
                ImplementationFactory,
                ServiceLifetime.Transient,
                proxyGenerationOptions,
                null));
        }

        #endregion

        #region AddDynamicProxyWithImplementationFactoryAndWithoutOptionsForInterfaceResolution

        [Test]
        public void AddDynamicProxyWithImplementationFactoryAndWithoutOptionsForInterfaceResolution()
        {
            var services = new ServiceCollection();
            TestService ImplementationFactory(IServiceProvider sp) => new TestService();
            IInterceptor[] interceptors = { new StandardInterceptor() };

            services.Add<ITestService, TestService>(
                ImplementationFactory,
                ServiceLifetime.Transient,
                interceptors);

            var serviceProvider = services.BuildServiceProvider();
            var service = serviceProvider.GetService<ITestService>();

            Assert.IsNotNull(service);
            Assert.IsInstanceOf<ITestService>(service);

            var proxyTargetAccessor = service as IProxyTargetAccessor;
            Assert.IsNotNull(proxyTargetAccessor);

            var actualInterceptors = proxyTargetAccessor.GetInterceptors();
            Assert.AreEqual(actualInterceptors.Length, interceptors.Length);
            Assert.AreEqual(actualInterceptors[0], interceptors[0]);

            var proxyTarget = proxyTargetAccessor.DynProxyGetTarget();
            Assert.IsInstanceOf<TestService>(proxyTarget);
        }

        [Test]
        public void AddDynamicProxyWithImplementationFactoryAndWithoutOptionsForInterfaceResolution_WhenInterceptorsIsNull_ThenThrowException()
        {
            var services = new ServiceCollection();
            TestService ImplementationFactory(IServiceProvider sp) => new TestService();

            Assert.Throws<ArgumentNullException>(() => services.Add<ITestService, TestService>(
                ImplementationFactory,
                ServiceLifetime.Transient,
                interceptors: null));
        }

        #endregion

        #region AddDynamicProxyWithOptionsForInterfaceResolution

        [Test]
        public void AddDynamicProxyWithOptionsForInterfaceResolution()
        {
            var services = new ServiceCollection();
            var proxyGenerationOptions = new ProxyGenerationOptions();
            IInterceptor[] interceptors = { new StandardInterceptor() };

            services.Add<ITestService, TestService>(
                ServiceLifetime.Transient,
                proxyGenerationOptions,
                interceptors);

            var serviceProvider = services.BuildServiceProvider();
            var service = serviceProvider.GetService<ITestService>();

            Assert.IsNotNull(service);
            Assert.IsInstanceOf<ITestService>(service);

            var proxyTargetAccessor = service as IProxyTargetAccessor;
            Assert.IsNotNull(proxyTargetAccessor);

            var actualInterceptors = proxyTargetAccessor.GetInterceptors();
            Assert.AreEqual(actualInterceptors.Length, interceptors.Length);
            Assert.AreEqual(actualInterceptors[0], interceptors[0]);

            var proxyTarget = proxyTargetAccessor.DynProxyGetTarget();
            Assert.IsInstanceOf<TestService>(proxyTarget);
        }

        #endregion

        #region AddDynamicProxyWithoutOptionsForInterfaceResolution

        [Test]
        public void AddDynamicProxyWithoutOptionsForInterfaceResolution()
        {
            var services = new ServiceCollection();
            IInterceptor[] interceptors = { new StandardInterceptor() };

            services.Add<ITestService, TestService>(
                ServiceLifetime.Transient,
                interceptors);

            var serviceProvider = services.BuildServiceProvider();
            var service = serviceProvider.GetService<ITestService>();

            Assert.IsNotNull(service);
            Assert.IsInstanceOf<ITestService>(service);

            var proxyTargetAccessor = service as IProxyTargetAccessor;
            Assert.IsNotNull(proxyTargetAccessor);

            var actualInterceptors = proxyTargetAccessor.GetInterceptors();
            Assert.AreEqual(actualInterceptors.Length, interceptors.Length);
            Assert.AreEqual(actualInterceptors[0], interceptors[0]);

            var proxyTarget = proxyTargetAccessor.DynProxyGetTarget();
            Assert.IsInstanceOf<TestService>(proxyTarget);
        }

        #endregion

        #endregion

        #region Class resolution

        #region AddDynamicProxyWithImplementationFactoryAndOptionsForClassResolution

        [Test]
        public void AddDynamicProxyWithImplementationFactoryAndOptionsForClassResolution()
        {
            var services = new ServiceCollection();
            TestService ImplementationFactory(IServiceProvider sp) => new TestService();
            var proxyGenerationOptions = new ProxyGenerationOptions();
            IInterceptor[] interceptors = { new StandardInterceptor() };

            services.Add(
                ImplementationFactory,
                ServiceLifetime.Transient,
                proxyGenerationOptions,
                interceptors);

            var serviceProvider = services.BuildServiceProvider();
            var service = serviceProvider.GetService<TestService>();

            Assert.IsNotNull(service);
            Assert.IsInstanceOf<TestService>(service);

            var proxyTargetAccessor = service as IProxyTargetAccessor;
            Assert.IsNotNull(proxyTargetAccessor);

            var actualInterceptors = proxyTargetAccessor.GetInterceptors();
            Assert.AreEqual(actualInterceptors.Length, interceptors.Length);
            Assert.AreEqual(actualInterceptors[0], interceptors[0]);

            var proxyTarget = proxyTargetAccessor.DynProxyGetTarget();
            Assert.IsInstanceOf<TestService>(proxyTarget);
        }

        [Test]
        public void AddDynamicProxyWithImplementationFactoryAndOptionsForClassResolution_WhenProxyGenerationOptionsIsNull_ThenThrowArgumentNullException()
        {
            var services = new ServiceCollection();
            TestService ImplementationFactory(IServiceProvider sp) => new TestService();
            IInterceptor[] interceptors = { new StandardInterceptor() };

            Assert.Throws<ArgumentNullException>(() => services.Add(
                ImplementationFactory,
                ServiceLifetime.Transient,
                null,
                interceptors));
        }

        [Test]
        public void AddDynamicProxyWithImplementationFactoryAndOptionsForClassResolution_WhenInterceptorsIsNull_ThenThrowArgumentNullException()
        {
            var services = new ServiceCollection();
            TestService ImplementationFactory(IServiceProvider sp) => new TestService();
            var proxyGenerationOptions = new ProxyGenerationOptions();

            Assert.Throws<ArgumentNullException>(() => services.Add(
                ImplementationFactory,
                ServiceLifetime.Transient,
                proxyGenerationOptions,
                null));
        }

        #endregion

        #region AddDynamicProxyWithImplementationFactoryAndWithoutOptionsForClassResolution

        [Test]
        public void AddDynamicProxyWithImplementationFactoryAndWithoutOptionsForClassResolution()
        {
            var services = new ServiceCollection();
            TestService ImplementationFactory(IServiceProvider sp) => new TestService();
            IInterceptor[] interceptors = { new StandardInterceptor() };

            services.Add(ImplementationFactory, ServiceLifetime.Transient, interceptors);

            var serviceProvider = services.BuildServiceProvider();
            var service = serviceProvider.GetService<TestService>();

            Assert.IsNotNull(service);
            Assert.IsInstanceOf<TestService>(service);

            var proxyTargetAccessor = service as IProxyTargetAccessor;
            Assert.IsNotNull(proxyTargetAccessor);

            var actualInterceptors = proxyTargetAccessor.GetInterceptors();
            Assert.AreEqual(actualInterceptors.Length, interceptors.Length);
            Assert.AreEqual(actualInterceptors[0], interceptors[0]);

            var proxyTarget = proxyTargetAccessor.DynProxyGetTarget();
            Assert.IsInstanceOf<TestService>(proxyTarget);
        }

        [Test]
        public void AddDynamicProxyWithImplementationFactoryAndWithoutOptionsForClassResolution_WhenInterceptorsIsNull_ThenThrowArgumentNullException()
        {
            var services = new ServiceCollection();
            TestService ImplementationFactory(IServiceProvider sp) => new TestService();

            Assert.Throws<ArgumentNullException>(() => services.Add(
                ImplementationFactory,
                ServiceLifetime.Transient,
                null));
        }

        #endregion

        #region AddDynamicProxyWithOptionsForClassResolution

        [Test]
        public void AddDynamicProxyWithOptionsForClassResolution()
        {
            var services = new ServiceCollection();
            var proxyGenerationOptions = new ProxyGenerationOptions();
            IInterceptor[] interceptors = { new StandardInterceptor() };

            services.Add<TestService>(
                ServiceLifetime.Transient,
                proxyGenerationOptions,
                interceptors);

            var serviceProvider = services.BuildServiceProvider();
            var service = serviceProvider.GetService<TestService>();

            Assert.IsNotNull(service);
            Assert.IsInstanceOf<TestService>(service);

            var proxyTargetAccessor = service as IProxyTargetAccessor;
            Assert.IsNotNull(proxyTargetAccessor);

            var actualInterceptors = proxyTargetAccessor.GetInterceptors();
            Assert.AreEqual(actualInterceptors.Length, interceptors.Length);
            Assert.AreEqual(actualInterceptors[0], interceptors[0]);

            var proxyTarget = proxyTargetAccessor.DynProxyGetTarget();
            Assert.IsInstanceOf<TestService>(proxyTarget);
        }

        #endregion

        #region AddDynamicProxyWithoutOptionsForClassResolution

        [Test]
        public void AddDynamicProxyWithoutOptionsForClassResolution()
        {
            var services = new ServiceCollection();

            IInterceptor[] interceptors = { new StandardInterceptor() };

            services.Add<TestService>(
                ServiceLifetime.Transient,
                interceptors);

            var serviceProvider = services.BuildServiceProvider();
            var service = serviceProvider.GetService<TestService>();

            Assert.IsNotNull(service);
            Assert.IsInstanceOf<TestService>(service);

            var proxyTargetAccessor = service as IProxyTargetAccessor;
            Assert.IsNotNull(proxyTargetAccessor);

            var actualInterceptors = proxyTargetAccessor.GetInterceptors();
            Assert.AreEqual(actualInterceptors.Length, interceptors.Length);
            Assert.AreEqual(actualInterceptors[0], interceptors[0]);

            var proxyTarget = proxyTargetAccessor.DynProxyGetTarget();
            Assert.IsInstanceOf<TestService>(proxyTarget);
        }

        #endregion

        #endregion

        #region AddTransient

        [Test]
        public void AddTransientWithoutImplementationFactoryAndOptionsForClass()
        {
            var services = new ServiceCollection();
            IInterceptor[] interceptors = { new StandardInterceptor() };

            services.AddTransient<TestService>(interceptors);

            Assert.AreEqual(1, services.Count);
            Assert.AreEqual(ServiceLifetime.Transient, services[0].Lifetime);
        }

        [Test]
        public void AddTransientWithoutImplementationFactoryAndWithOptionsForClass()
        {
            var services = new ServiceCollection();
            IInterceptor[] interceptors = { new StandardInterceptor() };
            var proxyGenerationOptions = new ProxyGenerationOptions();

            services.AddTransient<TestService>(proxyGenerationOptions, interceptors);

            Assert.AreEqual(1, services.Count);
            Assert.AreEqual(ServiceLifetime.Transient, services[0].Lifetime);
        }

        [Test]
        public void AddTransientWithoutImplementationFactoryAndOptionsForInterface()
        {
            var services = new ServiceCollection();
            IInterceptor[] interceptors = { new StandardInterceptor() };

            services.AddTransient<ITestService, TestService>(interceptors);

            Assert.AreEqual(1, services.Count);
            Assert.AreEqual(ServiceLifetime.Transient, services[0].Lifetime);
        }

        [Test]
        public void AddTransientWithoutImplementationFactoryAndWithOptionsForInterface()
        {
            var services = new ServiceCollection();
            IInterceptor[] interceptors = { new StandardInterceptor() };
            var proxyGenerationOptions = new ProxyGenerationOptions();

            services.AddTransient<ITestService, TestService>(proxyGenerationOptions, interceptors);

            Assert.AreEqual(1, services.Count);
            Assert.AreEqual(ServiceLifetime.Transient, services[0].Lifetime);
        }

        [Test]
        public void AddTransientWithImplementationFactoryAndWithoutOptionsForClass()
        {
            var services = new ServiceCollection();
            IInterceptor[] interceptors = { new StandardInterceptor() };
            TestService ImplementationFactory(IServiceProvider sp) => new TestService();

            services.AddTransient(ImplementationFactory, interceptors);

            Assert.AreEqual(1, services.Count);
            Assert.AreEqual(ServiceLifetime.Transient, services[0].Lifetime);
        }

        [Test]
        public void AddTransientWithImplementationFactoryAndOptionsForClass()
        {
            var services = new ServiceCollection();
            IInterceptor[] interceptors = { new StandardInterceptor() };
            TestService ImplementationFactory(IServiceProvider sp) => new TestService();
            var proxyGenerationOptions = new ProxyGenerationOptions();

            services.AddTransient(ImplementationFactory, proxyGenerationOptions, interceptors);

            Assert.AreEqual(1, services.Count);
            Assert.AreEqual(ServiceLifetime.Transient, services[0].Lifetime);
        }

        [Test]
        public void AddTransientWithImplementationFactoryAndWithoutOptionsForInterface()
        {
            var services = new ServiceCollection();
            IInterceptor[] interceptors = { new StandardInterceptor() };
            TestService ImplementationFactory(IServiceProvider sp) => new TestService();

            services.AddTransient<ITestService, TestService>(ImplementationFactory, interceptors);

            Assert.AreEqual(1, services.Count);
            Assert.AreEqual(ServiceLifetime.Transient, services[0].Lifetime);
        }

        [Test]
        public void AddTransientWithImplementationFactoryAndOptionsForInterface()
        {
            var services = new ServiceCollection();
            IInterceptor[] interceptors = { new StandardInterceptor() };
            TestService ImplementationFactory(IServiceProvider sp) => new TestService();
            var proxyGenerationOptions = new ProxyGenerationOptions();

            services.AddTransient<ITestService, TestService>(ImplementationFactory, proxyGenerationOptions, interceptors);

            Assert.AreEqual(1, services.Count);
            Assert.AreEqual(ServiceLifetime.Transient, services[0].Lifetime);
        }

        #endregion

        #region AddScoped

        [Test]
        public void AddScopedWithoutImplementationFactoryAndOptionsForClass()
        {
            var services = new ServiceCollection();
            IInterceptor[] interceptors = { new StandardInterceptor() };

            services.AddScoped<TestService>(interceptors);

            Assert.AreEqual(1, services.Count);
            Assert.AreEqual(ServiceLifetime.Scoped, services[0].Lifetime);
        }

        [Test]
        public void AddScopedWithoutImplementationFactoryAndWithOptionsForClass()
        {
            var services = new ServiceCollection();
            IInterceptor[] interceptors = { new StandardInterceptor() };
            var proxyGenerationOptions = new ProxyGenerationOptions();

            services.AddScoped<TestService>(proxyGenerationOptions, interceptors);

            Assert.AreEqual(1, services.Count);
            Assert.AreEqual(ServiceLifetime.Scoped, services[0].Lifetime);
        }

        [Test]
        public void AddScopedWithoutImplementationFactoryAndOptionsForInterface()
        {
            var services = new ServiceCollection();
            IInterceptor[] interceptors = { new StandardInterceptor() };

            services.AddScoped<ITestService, TestService>(interceptors);

            Assert.AreEqual(1, services.Count);
            Assert.AreEqual(ServiceLifetime.Scoped, services[0].Lifetime);
        }

        [Test]
        public void AddScopedWithoutImplementationFactoryAndWithOptionsForInterface()
        {
            var services = new ServiceCollection();
            IInterceptor[] interceptors = { new StandardInterceptor() };
            var proxyGenerationOptions = new ProxyGenerationOptions();

            services.AddScoped<ITestService, TestService>(proxyGenerationOptions, interceptors);

            Assert.AreEqual(1, services.Count);
            Assert.AreEqual(ServiceLifetime.Scoped, services[0].Lifetime);
        }

        [Test]
        public void AddScopedWithImplementationFactoryAndWithoutOptionsForClass()
        {
            var services = new ServiceCollection();
            IInterceptor[] interceptors = { new StandardInterceptor() };
            TestService ImplementationFactory(IServiceProvider sp) => new TestService();

            services.AddScoped(ImplementationFactory, interceptors);

            Assert.AreEqual(1, services.Count);
            Assert.AreEqual(ServiceLifetime.Scoped, services[0].Lifetime);
        }

        [Test]
        public void AddScopedWithImplementationFactoryAndOptionsForClass()
        {
            var services = new ServiceCollection();
            IInterceptor[] interceptors = { new StandardInterceptor() };
            TestService ImplementationFactory(IServiceProvider sp) => new TestService();
            var proxyGenerationOptions = new ProxyGenerationOptions();

            services.AddScoped(ImplementationFactory, proxyGenerationOptions, interceptors);

            Assert.AreEqual(1, services.Count);
            Assert.AreEqual(ServiceLifetime.Scoped, services[0].Lifetime);
        }

        [Test]
        public void AddScopedWithImplementationFactoryAndWithoutOptionsForInterface()
        {
            var services = new ServiceCollection();
            IInterceptor[] interceptors = { new StandardInterceptor() };
            TestService ImplementationFactory(IServiceProvider sp) => new TestService();

            services.AddScoped<ITestService, TestService>(ImplementationFactory, interceptors);

            Assert.AreEqual(1, services.Count);
            Assert.AreEqual(ServiceLifetime.Scoped, services[0].Lifetime);
        }

        [Test]
        public void AddScopedWithImplementationFactoryAndOptionsForInterface()
        {
            var services = new ServiceCollection();
            IInterceptor[] interceptors = { new StandardInterceptor() };
            TestService ImplementationFactory(IServiceProvider sp) => new TestService();
            var proxyGenerationOptions = new ProxyGenerationOptions();

            services.AddScoped<ITestService, TestService>(ImplementationFactory, proxyGenerationOptions, interceptors);

            Assert.AreEqual(1, services.Count);
            Assert.AreEqual(ServiceLifetime.Scoped, services[0].Lifetime);
        }

        #endregion

        #region AddSingleton

        [Test]
        public void AddSingletonWithoutImplementationFactoryAndOptionsForClass()
        {
            var services = new ServiceCollection();
            IInterceptor[] interceptors = { new StandardInterceptor() };

            services.AddSingleton<TestService>(interceptors);

            Assert.AreEqual(1, services.Count);
            Assert.AreEqual(ServiceLifetime.Singleton, services[0].Lifetime);
        }

        [Test]
        public void AddSingletonWithoutImplementationFactoryAndWithOptionsForClass()
        {
            var services = new ServiceCollection();
            IInterceptor[] interceptors = { new StandardInterceptor() };
            var proxyGenerationOptions = new ProxyGenerationOptions();

            services.AddSingleton<TestService>(proxyGenerationOptions, interceptors);

            Assert.AreEqual(1, services.Count);
            Assert.AreEqual(ServiceLifetime.Singleton, services[0].Lifetime);
        }

        [Test]
        public void AddSingletonWithoutImplementationFactoryAndOptionsForInterface()
        {
            var services = new ServiceCollection();
            IInterceptor[] interceptors = { new StandardInterceptor() };

            services.AddSingleton<ITestService, TestService>(interceptors);

            Assert.AreEqual(1, services.Count);
            Assert.AreEqual(ServiceLifetime.Singleton, services[0].Lifetime);
        }

        [Test]
        public void AddSingletonWithoutImplementationFactoryAndWithOptionsForInterface()
        {
            var services = new ServiceCollection();
            IInterceptor[] interceptors = { new StandardInterceptor() };
            var proxyGenerationOptions = new ProxyGenerationOptions();

            services.AddSingleton<ITestService, TestService>(proxyGenerationOptions, interceptors);

            Assert.AreEqual(1, services.Count);
            Assert.AreEqual(ServiceLifetime.Singleton, services[0].Lifetime);
        }

        [Test]
        public void AddSingletonWithImplementationFactoryAndWithoutOptionsForClass()
        {
            var services = new ServiceCollection();
            IInterceptor[] interceptors = { new StandardInterceptor() };
            TestService ImplementationFactory(IServiceProvider sp) => new TestService();

            services.AddSingleton(ImplementationFactory, interceptors);

            Assert.AreEqual(1, services.Count);
            Assert.AreEqual(ServiceLifetime.Singleton, services[0].Lifetime);
        }

        [Test]
        public void AddSingletonWithImplementationFactoryAndOptionsForClass()
        {
            var services = new ServiceCollection();
            IInterceptor[] interceptors = { new StandardInterceptor() };
            TestService ImplementationFactory(IServiceProvider sp) => new TestService();
            var proxyGenerationOptions = new ProxyGenerationOptions();

            services.AddSingleton(ImplementationFactory, proxyGenerationOptions, interceptors);

            Assert.AreEqual(1, services.Count);
            Assert.AreEqual(ServiceLifetime.Singleton, services[0].Lifetime);
        }

        [Test]
        public void AddSingletonWithImplementationFactoryAndWithoutOptionsForInterface()
        {
            var services = new ServiceCollection();
            IInterceptor[] interceptors = { new StandardInterceptor() };
            TestService ImplementationFactory(IServiceProvider sp) => new TestService();

            services.AddSingleton<ITestService, TestService>(ImplementationFactory, interceptors);

            Assert.AreEqual(1, services.Count);
            Assert.AreEqual(ServiceLifetime.Singleton, services[0].Lifetime);
        }

        [Test]
        public void AddSingletonWithImplementationFactoryAndOptionsForInterface()
        {
            var services = new ServiceCollection();
            IInterceptor[] interceptors = { new StandardInterceptor() };
            TestService ImplementationFactory(IServiceProvider sp) => new TestService();
            var proxyGenerationOptions = new ProxyGenerationOptions();

            services.AddSingleton<ITestService, TestService>(ImplementationFactory, proxyGenerationOptions, interceptors);

            Assert.AreEqual(1, services.Count);
            Assert.AreEqual(ServiceLifetime.Singleton, services[0].Lifetime);
        }

        #endregion
    }
}