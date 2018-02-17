using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Html;

namespace Incoding.Web.Extensions
{
    public static class WebStringExtensions
    {
        public static HtmlString ToMvcHtmlString(this string value)
        {
            return new HtmlString(value);
        }

        public static string HtmlContentToString(this IHtmlContent content)
        {
            using (var writer = new System.IO.StringWriter())
            {
                content.WriteTo(writer, HtmlEncoder.Default);
                return writer.ToString();
            }
        }
    }
}