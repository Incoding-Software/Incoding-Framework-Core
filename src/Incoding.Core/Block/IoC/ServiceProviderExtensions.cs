using System;
using Microsoft.Extensions.DependencyInjection;

namespace Incoding.Core.Block.IoC
{
    public static class ServiceProviderExtensions
    {
        public static TService GetService<TService>(this IServiceProvider serviceProvider, object name)
            where TService : class
        {
            return serviceProvider
                .GetService<NamedServiceProvider>()
                .Resolve<TService>(serviceProvider, typeof(TService), name);
        }
    }
}