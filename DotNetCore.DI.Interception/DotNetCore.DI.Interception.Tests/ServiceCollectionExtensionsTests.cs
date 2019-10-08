using System;
using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using IInvocation = Castle.DynamicProxy.IInvocation;

namespace DotNetCore.DI.Interception.Tests
{
    public class ServiceCollectionExtensionsTests
    {
        #region Add

        [Test]
        public void Add()
        {
            var services = new ServiceCollection();
            TestService ImplementationFactory(IServiceProvider sp) => new TestService();
            ITestService ProxyFactory(TestService s) => new TestProxyService(s);

            ServiceCollectionExtensions.Add<ITestService, TestService>(
                services,
                ImplementationFactory,
                ServiceLifetime.Transient,
                ProxyFactory);

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
                ServiceCollectionExtensions.Add<ITestService, TestService>(
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
                ServiceCollectionExtensions.Add<ITestService, TestService>(
                    services,
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
                ServiceCollectionExtensions.Add<ITestService, TestService>(
                    services,
                    ImplementationFactory,
                    ServiceLifetime.Transient,
                    proxyFactory: null));
        }

        #endregion

        #region AddDynamicProxyWithOptions

        [Test]
        public void AddDynamicProxyWithOptions()
        {
            var services = new ServiceCollection();
            TestService ImplementationFactory(IServiceProvider sp) => new TestService();
            var proxyGenerationOptions = new ProxyGenerationOptions();
            IInterceptor[] interceptors = { new StandardInterceptor() };

            ServiceCollectionExtensions.Add<ITestService, TestService>(
                services,
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
        public void AddDynamicProxyWithOptions_WhenProxyGenerationOptionsIsNull_ThenThrowException()
        {
            var services = new ServiceCollection();
            TestService ImplementationFactory(IServiceProvider sp) => new TestService();
            IInterceptor[] interceptors = { new StandardInterceptor() };

            Assert.Throws<ArgumentNullException>(() => ServiceCollectionExtensions.Add<ITestService, TestService>(
                   services,
                   ImplementationFactory,
                   ServiceLifetime.Transient,
                   null,
                   interceptors));
        }

        [Test]
        public void AddDynamicProxyWithOptions_WhenInterceptorsIsNull_ThenThrowException()
        {
            var services = new ServiceCollection();
            TestService ImplementationFactory(IServiceProvider sp) => new TestService();
            var proxyGenerationOptions = new ProxyGenerationOptions();

            Assert.Throws<ArgumentNullException>(() => ServiceCollectionExtensions.Add<ITestService, TestService>(
                   services,
                   ImplementationFactory,
                   ServiceLifetime.Transient,
                   proxyGenerationOptions,
                   null));
        }

        #endregion

        #region AddDynamicProxyWithoutOptions

        [Test]
        public void AddDynamicProxyWithoutOptions()
        {
            var services = new ServiceCollection();
            TestService ImplementationFactory(IServiceProvider sp) => new TestService();
            IInterceptor[] interceptors = { new StandardInterceptor() };

            ServiceCollectionExtensions.Add<ITestService, TestService>(
                services,
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
        public void AddDynamicProxyWithoutOptions_WhenInterceptorsIsNull_ThenThrowException()
        {
            var services = new ServiceCollection();
            TestService ImplementationFactory(IServiceProvider sp) => new TestService();

            Assert.Throws<ArgumentNullException>(() => ServiceCollectionExtensions.Add<ITestService, TestService>(
                   services,
                   ImplementationFactory,
                   ServiceLifetime.Transient,
                   interceptors: null));
        }

        #endregion
    }
}