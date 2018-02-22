using System.Collections.Concurrent;

namespace Incoding.Web.Grid
{
    public class GridTemplate
    {
        public static readonly ConcurrentDictionary<string, string> Templates = new ConcurrentDictionary<string, string>();
        public const string TemplateIdKey = "templateId";
        public const string RouteName = "IncodingGridTemplate";

        public static string BuildTemplateUrl(string templateId)
        {
            return $"/{RouteName}?{TemplateIdKey}={templateId}";
        }
    }
}