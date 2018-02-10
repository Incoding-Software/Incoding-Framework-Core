using Incoding.CQRS;
using Microsoft.Extensions.DependencyInjection;

namespace Incoding.Core
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureIncodingCoreServices(this IServiceCollection services)
        {
            services.AddTransient<IDispatcher, DefaultDispatcher>();
        }
    }
}