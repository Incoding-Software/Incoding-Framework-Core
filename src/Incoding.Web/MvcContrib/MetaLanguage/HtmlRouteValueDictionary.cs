using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;

namespace Incoding.Web.MvcContrib
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