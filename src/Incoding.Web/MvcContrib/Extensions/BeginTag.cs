using System;
using System.Text.Encodings.Web;
using Incoding.Core.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;

namespace Incoding.Web.MvcContrib
{
    #region << Using >>

    #endregion

    public class  BeginTag : IDisposable
    {
        #region Fields

        readonly IHtmlHelper htmlHelper;
        private readonly string _tag;

        #endregion

        #region Constructors

        public BeginTag(IHtmlHelper htmlHelper, HtmlTag tag, RouteValueDictionary attributes) : this(htmlHelper, tag.ToStringLower(), attributes)
        {
        }

        public BeginTag(IHtmlHelper htmlHelper, string tag, RouteValueDictionary attributes)
        {
            this.htmlHelper = htmlHelper;
            _tag = tag;
            TagBuilder tagBuilder = new TagBuilder(tag);
            tagBuilder.MergeAttributes(attributes);
            tagBuilder.TagRenderMode = TagRenderMode.StartTag;
            tagBuilder.WriteTo(htmlHelper.ViewContext.Writer, HtmlEncoder.Default);
        }

        #endregion

        #region Disposable

        public void Dispose()
        {
            TagBuilder tagBuilder = new TagBuilder(_tag);
            tagBuilder.TagRenderMode = TagRenderMode.EndTag;
            tagBuilder.WriteTo(htmlHelper.ViewContext.Writer, HtmlEncoder.Default);
        }

        #endregion
    }
}