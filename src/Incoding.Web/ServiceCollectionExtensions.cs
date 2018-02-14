using Incoding.Block.Caching;
using Incoding.Mvc.MvcContrib.Template.Factory;
using Incoding.Web.MvcContrib.IncHtmlHelper;
using Incoding.Web.MvcContrib.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Caching.Memory;
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