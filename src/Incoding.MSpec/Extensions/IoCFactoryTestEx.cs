using System;
using System.Collections.Generic;
using System.Linq;
using Incoding.Core.Block.IoC;
using Incoding.Core.Block.IoC.Core;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace Incoding.MSpecContrib
{

    public static class IoCFactoryTestEx
    {
        #region Factory constructors

        public static void Stub(this IoCFactory factory, Action<Mock<IIoCProvider>> action)
        {
            factory.Initialize(init => StubInit(init, action));
        }
        
        public static void StubResolveAll<TInstance>(this IoCFactory factory, IEnumerable<TInstance> mockInstances)
        {
            Stub(factory, s => s.Setup(r => r.GetAll<TInstance>(typeof(TInstance))).Returns(mockInstances));
        }

        public static void StubTryResolve<TInstance>(this IoCFactory factory, TInstance mockInstance) where TInstance : class
        {
            Stub(factory, s => s.Setup(r => r.TryGet<TInstance>()).Returns(mockInstance));
        }

        public static void StubTryResolveByNamed<TInstance>(this IoCFactory factory, string named, TInstance mockInstance) where TInstance : class
        {
            Stub(factory, s => s.Setup(r => r.TryGetByNamed<TInstance>(named)).Returns(mockInstance));
        }

        #endregion

        static void StubInit(IoCInit init, Action<Mock<IIoCProvider>> action)
        {
            var mockProvider = Pleasure.Mock(action);
            if (init.Provider != null)
            {
                try
                {
                    mockProvider = Mock.Get(init.Provider);
                    action(mockProvider);
                }

                ////ncrunch: no coverage start
                catch (Exception)
                {
                    mockProvider = Pleasure.Mock(action);
                }

                ////ncrunch: no coverage end
            }

            init.WithProvider(mockProvider.Object);
        }
    }
    /*
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
            Stub(factory, s => s.Setup(r => r.GetServices(typeof(TInstance))).Returns(mockInstances.OfType<object>()));
        }

        public static void StubTryResolve<TInstance>(this IoCFactory factory, TInstance mockInstance) where TInstance : class
        {
            Stub(factory, s => s.Setup(r => r.GetService(typeof(TInstance))).Returns(mockInstance));
        }

        public static void StubTryResolveByNamed<TInstance>(this IoCFactory factory, string named, TInstance mockInstance) where TInstance : class
        {
            //TODO: would throw error cause GetService is extension here
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
    }*/
}