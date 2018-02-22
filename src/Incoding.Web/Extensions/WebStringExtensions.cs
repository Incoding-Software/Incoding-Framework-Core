using System.Text;
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

        public static HtmlString ToHtmlString(this IHtmlContent value)
        {
            return new HtmlString(value.HtmlContentToString());
        }

        public static string HtmlContentToString(this IHtmlContent content)
        {
            using (var writer = new System.IO.StringWriter())
            {
                content.WriteTo(writer, HtmlEncoder.Default);
                return writer.ToString();
            }
        }

        public static HtmlString Concat(this HtmlString first, string plainString)
        {
            return Concat(first, new HtmlString(plainString));
        }

        public static HtmlString Concat(this HtmlString first, IHtmlContent htmlContent)
        {
            return Concat(first, htmlContent.HtmlContentToString());
        }

        public static HtmlString Concat(this HtmlString first, params HtmlString[] htmlStringsForConcat)
        {
            var sb = new StringBuilder();
            sb.Append(first);
            foreach (var htmlString in htmlStringsForConcat)
            {
                sb.Append(htmlString);
            }
            return new HtmlString(sb.ToString());
        }

        public static HtmlString Concat(this HtmlString first, params string[] stringsForConcat)
        {
            var sb = new StringBuilder();
            sb.Append(first);
            foreach (var htmlString in stringsForConcat)
            {
                sb.Append(htmlString);
            }
            return new HtmlString(sb.ToString());
        }

    }
}