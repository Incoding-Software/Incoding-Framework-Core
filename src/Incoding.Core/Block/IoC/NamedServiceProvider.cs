using System;
using System.Collections.Generic;
using System.Reflection;

namespace Incoding.Core.Block.IoC
{
    public class NamedServiceProvider
    {
        private readonly Dictionary<Type, Dictionary<string, object>> serviceNameMap =
            new Dictionary<Type, Dictionary<string, object>>();

        public void RegisterType<TImplementation>(Type service, object implementation, string name)
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
                this.serviceNameMap.Add(service, new Dictionary<string, object>
                {
                    [name] = implementation
                });
            }
        }

        public TService Resolve<TService>(IServiceProvider serviceProvider, Type serviceType, string name) where TService : class
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
            return serviceProvider.GetService<TService>(this.serviceNameMap[service][name]) as TService;
        }
    }
}