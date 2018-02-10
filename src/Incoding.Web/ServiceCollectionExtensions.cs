using Incoding.Web.MvcContrib.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Incoding.Web
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureIncodingWebServices(this IServiceCollection services)
        {
            services.AddScoped<IViewRenderService, ViewRenderService>();
        }
    }
}