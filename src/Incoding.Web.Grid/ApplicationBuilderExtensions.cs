using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Incoding.Web.Grid
{
    public static class ApplicationBuilderExtensions
    {
        public static void MapGridTemplates(this IApplicationBuilder appBuilder)
        {
            appBuilder.Map($"/{GridTemplate.RouteName}", builder => builder.Run(context =>
            {
                var templateId = context.Request.Query[GridTemplate.TemplateIdKey].ToString();
                if(GridTemplate.Templates.ContainsKey(templateId))
                    return context.Response.WriteAsync(GridTemplate.Templates[templateId]);
                return context.Response.WriteAsync(string.Empty);
            }));
        }
    }
}