using System;
using System.IO;
using System.Linq.Expressions;
using System.Text.Encodings.Web;
using Incoding.Core.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Incoding.Web.MvcContrib
{
    #region << Using >>

    #endregion

    public class IncFileControl<TModel, TProperty> : IncControlBase<TModel> where TModel : new()
    {
        #region Fields
        
        #endregion

        #region Constructors

        public IncFileControl(IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> property) : base(htmlHelper)
        {
            this.attributes.Set("id", ReflectionExtensions.GetMemberNameAsHtmlId(property));
            this.attributes.Set("name", ReflectionExtensions.GetMemberName(property));
        }

        #endregion

        #region Properties

        public string Value { get; set; }

        #endregion
        
        public override void WriteTo(TextWriter writer, HtmlEncoder encoder)
        {
            this.htmlHelper.Incoding().File(Value, GetAttributes()).WriteTo(writer, encoder);
        }
    }
}