using System;
using System.Collections.Generic;
using System.Linq;
using Incoding.Core.Block.IoC.Core;
using Microsoft.Extensions.DependencyInjection;

namespace Incoding.Core.Block.IoC.Provider
{
    #region << Using >>

    #endregion

    public class MSDependencyInjectionIoCProvider : IIoCProvider
    {
        private readonly IServiceProvider container;
        
        public MSDependencyInjectionIoCProvider(IServiceProvider container)
        {
            this.container = container;
        }

        public void Dispose()
        {
        }

        public void Eject<TInstance>()
        {
            throw new NotSupportedException();
        }

        public void Forward<TNew>(TNew newInstance) where TNew : class
        {
            throw new NotSupportedException();
        }
        
        //public TInstance Get<TInstance>(Type type) where TInstance : class
        //{
        //    return (TInstance)this.container.GetRequiredService(type);
        //}

        public IEnumerable<TInstance> GetAll<TInstance>(Type typeInstance)
        {
            try
            {
                return this.container.GetServices(typeInstance).OfType<TInstance>();
            }
            catch (Exception)
            {
                return new TInstance[0];
            }
        }

        public TInstance TryGet<TInstance>() where TInstance : class
        {
            TInstance instance;
            try
            {
                instance = this.container.GetRequiredService<TInstance>();
            }
            catch
            {
                instance = null;
            }
            return instance;
        }

        public TInstance TryGet<TInstance>(Type type) where TInstance : class
        {
            TInstance instance;
            try
            {
                instance = (TInstance)this.container.GetRequiredService(type);
            }
            catch
            {
                instance = null;
            }
            return instance;
        }

        public TInstance TryGetByNamed<TInstance>(object named) where TInstance : class
        {
            throw new NotImplementedException("Not implemented resolve by name yet");
            //return this.container.GetService<TInstance>(named);
        }
    }
}