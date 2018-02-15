using System;
using System.Collections.Generic;
using System.Reflection;

namespace Incoding.Core.Block.IoC
{
    public class NamedServiceProvider
    {
        private readonly Dictionary<Type, Dictionary<object, Type>> serviceNameMap =
            new Dictionary<Type, Dictionary<object, Type>>();

        public void RegisterType(Type service, Type implementation, object name)
        {
            if (this.serviceNameMap.ContainsKey(service))
            {
                var serviceNames = serviceNameMap[service];
                if (serviceNames.ContainsKey(name))
                {
                    /* overwrite existing name implementation */
                    serviceNames[name] = implementation;
                }
                else
                {
                    serviceNames.Add(name, implementation);
                }
            }
            else
            {
                this.serviceNameMap.Add(service, new Dictionary<object, Type>
                {
                    [name] = implementation
                });
            }
        }

        public TService Resolve<TService>(IServiceProvider serviceProvider, Type serviceType, object name) where TService : class
        {
            var service = serviceType;
            //if (service.GetTypeInfo().IsGenericType)
            //{
            //    return this.ResolveGeneric(serviceProvider, serviceType, name);
            //}
            var serviceExists = this.serviceNameMap.ContainsKey(service);
            var nameExists = serviceExists && this.serviceNameMap[service].ContainsKey(name);
            /* Return `null` if there is no mapping for either service type or requested name */
            if (!(serviceExists && nameExists))
            {
                return null;
            }
            return serviceProvider.GetService(this.serviceNameMap[service][name]) as TService;
        }
    }
}