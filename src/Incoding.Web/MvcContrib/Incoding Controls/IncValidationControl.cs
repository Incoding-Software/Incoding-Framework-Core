using System.IO;
using System.Linq.Expressions;
using System.Text.Encodings.Web;
using Incoding.Extensions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Incoding.Mvc.MvcContrib.Incoding_Controls
{
    #region << Using >>

    #endregion

    public class IncValidationControl<TModel> : IncControlBase<TModel>
    {
        #region Fields

        readonly IHtmlHelper htmlHelper;

        readonly string property;

        #endregion

        #region Constructors

        public IncValidationControl(IHtmlHelper htmlHelper, LambdaExpression property)
        {
            this.htmlHelper = htmlHelper;
            this.property = ReflectionExtensions.GetMemberName(property);
        }

        #endregion
        
        public override void WriteTo(TextWriter writer, HtmlEncoder encoder)
        {
            this.htmlHelper.ValidationMessage(this.property, string.Empty, this.attributes, "span").WriteTo(writer, encoder);
        }
    }
}