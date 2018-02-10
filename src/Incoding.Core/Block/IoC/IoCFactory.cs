using System;
using System.Collections.Generic;
using System.Linq;
using Incoding.Core.Block.IoC;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Incoding.Block.IoC
{
    public class IoCFactory
    {
        IServiceProvider serviceProvider;
        bool initialized;

        static readonly Lazy<IoCFactory> instance = new Lazy<IoCFactory>(() => new IoCFactory());

        public static IoCFactory Instance { get { return instance.Value; } }

        public void Initialize([NotNull] IServiceProvider serviceProvider)
        {
            if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));
            this.serviceProvider = serviceProvider;
            initialized = true;
        }

        public bool IsInitialized { get { return initialized; } }

        public TInstance TryResolve<TInstance>() where TInstance : class
        {
            var resolve = this.serviceProvider.GetRequiredService<TInstance>();
            return resolve;
        }
        public TInstance TryResolveByNamed<TInstance>(string name) where TInstance : class
        {
            var resolve = this.serviceProvider.GetService<TInstance>(name);
            return resolve;
        }
        
        public TInstance TryResolve<TInstance>([NotNull] Type typeInstance) where TInstance : class
        {
            var resolve = this.serviceProvider.GetRequiredService(typeInstance);
            return resolve as TInstance;
        }

        public IEnumerable<TInstance> ResolveAll<TInstance>() where TInstance : class
        {
            return ResolveAll<TInstance>(typeof(TInstance));
        }

        public IEnumerable<TInstance> ResolveAll<TInstance>([NotNull] Type typeInstance) where TInstance : class
        {
            var allInstances = this.serviceProvider.GetServices(typeInstance).OfType<TInstance>();
            return allInstances;
        }

        public IServiceProvider GetProvider()
        {
            return serviceProvider;
        }
    }

    /*
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Incoding.Block.Core;
    using JetBrains.Annotations;

    #endregion

    public class IoCFactory : FactoryBase<IoCInit>
    {
        #region Static Fields

        static readonly Lazy<IoCFactory> instance = new Lazy<IoCFactory>(() => new IoCFactory());

        #endregion

        #region Properties

        public static IoCFactory Instance { get { return instance.Value; } }
         
        #endregion

        #region Api Methods

        public void Forward<TInstance>([NotNull] TInstance newInstance) where TInstance : class
        {
            this.init.Provider.Forward(newInstance);
        }

        public void Eject<TInstance>()
        {
            this.init.Provider.Eject<TInstance>();
        }

        [Obsolete("Please use TryResolve instead of Resolve because")]
        public TInstance Resolve<TInstance>([NotNull] Type typeInstance) where TInstance : class
        {
            Guard.NotNull("typeInstance", typeInstance);

            var resolve = this.init.Provider.Get<TInstance>(typeInstance);
            return resolve;
        }

        public List<TInstance> ResolveAll<TInstance>() where TInstance : class
        {
            return ResolveAll<TInstance>(typeof(TInstance));
        }

        public List<TInstance> ResolveAll<TInstance>([NotNull] Type typeInstance) where TInstance : class
        {
            var allInstances = this.init.Provider.GetAll<TInstance>(typeInstance).ToList();
            return allInstances;
        }

        public TInstance TryResolve<TInstance>() where TInstance : class
        {
            var resolve = this.init.Provider.TryGet<TInstance>();
            return resolve;
        }

        public TInstance TryResolveByNamed<TInstance>([NotNull] string named) where TInstance : class
        {
            var resolve = this.init.Provider.TryGetByNamed<TInstance>(named);
            return resolve;
        }

        public TInstance TryResolve<TInstance>([NotNull] Type typeInstance) where TInstance : class
        {
            var resolve = this.init.Provider.TryGet<TInstance>(typeInstance);
            return resolve;
        }

        #endregion
    }*/
}