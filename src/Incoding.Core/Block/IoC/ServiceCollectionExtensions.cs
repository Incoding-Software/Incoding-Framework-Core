using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Text;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Incoding.Core.Block.IoC
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddScoped<TService, TImplementation>(this IServiceCollection services, string name)
            where TService : class
            where TImplementation : class, TService
        {
            return services.Add(typeof(TService), typeof(TImplementation), ServiceLifetime.Scoped, name);
        }

        private static IServiceCollection Add(this IServiceCollection services, Type serviceType, Type implementationType, ServiceLifetime lifetime, string name)
        {
            var namedServiceProvider = services.GetOrCreateNamedServiceProvider();

            namedServiceProvider.RegisterType(serviceType, implementationType, name);

            services.TryAddSingleton(namedServiceProvider);
            services.Add(new ServiceDescriptor(implementationType, implementationType, lifetime));

            return services;
        }

        private static NamedServiceProvider GetOrCreateNamedServiceProvider(this IServiceCollection services)
        {
            return services.FirstOrDefault(descriptor =>
                       descriptor.ServiceType == typeof(NamedServiceProvider))?.ImplementationInstance as NamedServiceProvider
                   ?? new NamedServiceProvider();
        }
    }
}
