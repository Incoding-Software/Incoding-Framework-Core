using Microsoft.AspNetCore.Html;

namespace Incoding.Extensions
{
    public static class WebStringExtensions
    {
        public static HtmlString ToMvcHtmlString(this string value)
        {
            return new HtmlString(value);
        }
    }
}