using System.IO;
using System.Text.Encodings.Web;
using Incoding.Extensions;
using Incoding.Mvc.MvcContrib.Primitive;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Incoding.Mvc.MvcContrib.Incoding_Controls
{
    #region << Using >>

    #endregion

    public class IncHelpBlockControl<TModel> : IncControlBase<TModel>
    {
        #region Properties

        public string Message { get; set; }

        #endregion
        
        public override void WriteTo(TextWriter writer, HtmlEncoder encoder)
        {
            if (string.IsNullOrWhiteSpace(Message))
                return;

            var p = new TagBuilder(HtmlTag.P.ToStringLower());
            p.MergeAttributes(this.attributes, true);
            p.AddCssClass("help-block");
            p.InnerHtml.Append(Message);
            p.WriteTo(writer, encoder);
        }
    }
}