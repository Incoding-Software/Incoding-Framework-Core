using Incoding.Extensions;
using Incoding.Mvc.MvcContrib.Primitive;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Incoding.Mvc.MvcContrib.Incoding_Controls
{
    public class IncDivControl<TModel> : IncControlBase<TModel>
    {
        public HtmlString Content { get; internal set; }


        public override IHtmlContent ToHtmlString()
        {
            var tagBuilder = new TagBuilder(HtmlTag.Div.ToStringLower());
            tagBuilder.TagRenderMode = TagRenderMode.Normal;
            tagBuilder.InnerHtml.AppendHtml(Content);

            tagBuilder.MergeAttributes(this.attributes, true);
            return new HtmlString(tagBuilder.ToString());
        }
    }
}