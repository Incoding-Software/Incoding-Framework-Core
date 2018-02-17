using System.IO;
using System.Text.Encodings.Web;
using Incoding.Core.Extensions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Incoding.Web.MvcContrib
{
    public class IncDivControl<TModel> : IncControlBase<TModel>
    {
        public IHtmlContent Content { get; internal set; }
        
        public override void WriteTo(TextWriter writer, HtmlEncoder encoder)
        {
            var tagBuilder = new TagBuilder(HtmlTag.Div.ToStringLower());
            tagBuilder.TagRenderMode = TagRenderMode.Normal;
            tagBuilder.InnerHtml.AppendHtml(Content);

            tagBuilder.MergeAttributes(this.attributes, true);
            tagBuilder.WriteTo(writer, encoder);
        }

        public IncDivControl(IHtmlHelper<TModel> htmlHelper) : base(htmlHelper)
        {
        }
    }
}