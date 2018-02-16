using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Incoding.Mvc.MvcContrib.Extensions
{
    public static class RouteExtensions
    {
        public static void ConfigureCQRS(this IRouteBuilder routeBuilder)
        {
            routeBuilder.MapRoute(
                name: "incodingCqrsQuery",
                template: "Cqrs/Query/{incType}",
                defaults: new {controller = "Dispatcher", action = "Query"});
            routeBuilder.MapRoute(
                name: "incodingCqrsValidate",
                template: "Cqrs/Query/{incType}",
                defaults: new {controller = "Dispatcher", action = "Validate" });
            routeBuilder.MapRoute(
                name: "incodingCqrsCommand",
                template: "Cqrs/Command/{incTypes}",
                defaults: new {controller = "Dispatcher", action = "Push"});
            routeBuilder.MapRoute(
                name: "incodingCqrsRender",
                template: "Cqrs/Render/{incView}",
                defaults: new {controller = "Dispatcher", action = "Render"});
            routeBuilder.MapRoute(
                name: "incodingCqrsFile",
                template: "Cqrs/File/{incType}/{incFileDownloadName}",
                defaults: new {controller = "Dispatcher", action = "QueryToFile"});
        }
    }
}