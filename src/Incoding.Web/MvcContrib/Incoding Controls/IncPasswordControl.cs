using System;
using System.IO;
using System.Linq.Expressions;
using System.Text.Encodings.Web;
using Incoding.Core.Extensions;
using Incoding.Extensions;
using Incoding.Mvc.MvcContrib.Incoding_Meta_Language.JqueryHelper.Primitive;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Incoding.Mvc.MvcContrib.Incoding_Controls
{
    #region << Using >>

    #endregion

    public class IncPasswordControl<TModel, TProperty> : IncControlBase<TModel>
    {
        #region Fields
        
        readonly Expression<Func<TModel, TProperty>> property;

        #endregion

        public int MaxLength { set { this.attributes.Set(HtmlAttribute.MaxLength.ToStringLower(), value); } }

        public string Placeholder { set { this.attributes.Set(HtmlAttribute.Placeholder.ToStringLower(), value); } }

        #region Constructors

        public IncPasswordControl(IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> property) : base(htmlHelper)
        {
            this.property = property;
        }

        #endregion
        
        public override void WriteTo(TextWriter writer, HtmlEncoder encoder)
        {
            this.htmlHelper.PasswordFor(this.property, GetAttributes()).WriteTo(writer, encoder);
        }
    }
}