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
    }
}