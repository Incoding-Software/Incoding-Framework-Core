using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;

namespace Incoding.Mvc.MvcContrib.Incoding_Meta_Language
{
    public class HtmlRouteValueDictionary : RouteValueDictionary
    {
        public HtmlRouteValueDictionary()
        {
        }

        public HtmlRouteValueDictionary(object values) : base(values)
        {
        }

        public IHtmlHelper HtmlHelper { get; set; }

    }
}