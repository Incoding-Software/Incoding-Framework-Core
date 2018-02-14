using System;
using System.Linq.Expressions;
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

        readonly IHtmlHelper<TModel> htmlHelper;

        readonly Expression<Func<TModel, TProperty>> property;

        #endregion

        public int MaxLength { set { this.attributes.Set(HtmlAttribute.MaxLength.ToStringLower(), value); } }

        public string Placeholder { set { this.attributes.Set(HtmlAttribute.Placeholder.ToStringLower(), value); } }

        #region Constructors

        public IncPasswordControl(IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> property)
        {
            this.htmlHelper = htmlHelper;
            this.property = property;
        }

        #endregion

        public override IHtmlContent ToHtmlString()
        {
            return this.htmlHelper.PasswordFor(this.property, GetAttributes());
        }
    }
}