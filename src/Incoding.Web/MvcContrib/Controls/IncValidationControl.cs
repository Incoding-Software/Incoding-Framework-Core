using System.IO;
using System.Linq.Expressions;
using System.Text.Encodings.Web;
using Incoding.Core.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Incoding.Web.MvcContrib
{
    #region << Using >>

    #endregion

    public class IncValidationControl<TModel> : IncControlBase<TModel>
    {
        #region Fields
        
        readonly string property;

        #endregion

        #region Constructors

        public IncValidationControl(IHtmlHelper<TModel> htmlHelper, LambdaExpression property) : base(htmlHelper)
        {
            this.property = ReflectionExtensions.GetMemberName(property);
        }

        #endregion
        
        public override void WriteTo(TextWriter writer, HtmlEncoder encoder)
        {
            this.htmlHelper.ValidationMessage(this.property, string.Empty, this.attributes, "span").WriteTo(writer, encoder);
        }
    }
}