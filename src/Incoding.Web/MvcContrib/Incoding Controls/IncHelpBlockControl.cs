using Incoding.Extensions;
using Incoding.Mvc.MvcContrib.Primitive;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Incoding.Mvc.MvcContrib.Incoding_Controls
{
    #region << Using >>

    #endregion

    public class IncHelpBlockControl : IncControlBase
    {
        #region Properties

        public string Message { get; set; }

        #endregion

        public override IHtmlContent ToHtmlString()
        {
            if (string.IsNullOrWhiteSpace(Message))
                return HtmlString.Empty;

            var p = new TagBuilder(HtmlTag.P.ToStringLower());
            p.MergeAttributes(this.attributes, true);
            p.AddCssClass("help-block");
            p.InnerHtml.Append(Message);
            return new HtmlString(p.ToString());
        }
    }
}