using Incoding.Core.Block.IoC;
using Microsoft.Extensions.DependencyInjection;

namespace Incoding.MSpecContrib
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using Incoding.Block.IoC;
    using Moq;

    #endregion

    public static class IoCFactoryTestEx
    {
        #region Factory constructors
        
        public static void Stub(this IoCFactory factory, Action<Mock<IServiceProvider>> action)
        {
            factory.Initialize(StubInit(factory, action));
        }

        public static void StubResolve<TInstance>(this IoCFactory factory, Type type, TInstance mockInstance) where TInstance : class
        {
            Stub(factory, s => s.Setup(r => r.GetService(type)).Returns(mockInstance));
        }

        public static void StubResolveAll<TInstance>(this IoCFactory factory, IEnumerable<TInstance> mockInstances)
        {
            Stub(factory, s => s.Setup(r => r.GetServices<TInstance>()).Returns(mockInstances));
        }

        public static void StubTryResolve<TInstance>(this IoCFactory factory, TInstance mockInstance) where TInstance : class
        {
            Stub(factory, s => s.Setup(r => r.GetService<TInstance>()).Returns(mockInstance));
        }

        public static void StubTryResolveByNamed<TInstance>(this IoCFactory factory, string named, TInstance mockInstance) where TInstance : class
        {
            Stub(factory, s => s.Setup(r => r.GetService<TInstance>(named)).Returns(mockInstance));
        }

        #endregion

        static IServiceProvider StubInit(IoCFactory factory, Action<Mock<IServiceProvider>> action)
        {
            var mockProvider = Pleasure.Mock(action);
            if (factory.IsInitialized)
            {
                try
                {
                    mockProvider = Mock.Get(factory.GetProvider());
                    action(mockProvider);
                }
                        
                        ////ncrunch: no coverage start
                catch (Exception)
                {
                    mockProvider = Pleasure.Mock(action);
                }

                ////ncrunch: no coverage end
            }

            factory.Initialize(mockProvider.Object);
            return mockProvider.Object;
        }
    }
}