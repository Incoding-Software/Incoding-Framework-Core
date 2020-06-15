using Incoding.Web.MvcContrib;
using Microsoft.Extensions.DependencyInjection;

namespace Incoding.Web
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureIncodingWebServices(this IServiceCollection services)
        {
            services.AddScoped<IViewRenderService, ViewRenderService>();
            services.AddMemoryCache();
            services.AddTransient<ITemplateFactory, TemplateHandlebarsFactory>();
            IncodingHtmlHelper.BootstrapVersion = BootstrapOfVersion.v3;

        }
    }
}