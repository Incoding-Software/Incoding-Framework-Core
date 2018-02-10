using System;
using System.Text;
using System.Web;
using Incoding.Extensions;
using Incoding.Mvc.MvcContrib.Primitive;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;

namespace Incoding.Mvc.MvcContrib.Extensions
{
    #region << Using >>

    #endregion

    public class  BeginTag : IDisposable
    {
        #region Fields

        readonly IHtmlHelper htmlHelper;

        readonly string tag;

        #endregion

        #region Constructors

        public BeginTag(IHtmlHelper htmlHelper, HtmlTag tag, RouteValueDictionary attributes)
        {
            this.htmlHelper = htmlHelper;
            this.tag = tag.ToStringLower();
            
            var startTag = new StringBuilder();
            startTag.Append("<{0} ".F(this.tag));
            foreach (var attr in attributes)
                startTag.Append("{0}=\"{1}\" ".F(attr.Key, HttpUtility.HtmlEncode(attr.Value)));
            startTag.Append(">");
            htmlHelper.ViewContext.Writer.Write(startTag.ToString());
        }

        #endregion

        #region Disposable

        public void Dispose()
        {
            this.htmlHelper.ViewContext.Writer.Write("</{0}>".F(this.tag));
        }

        #endregion
    }
}