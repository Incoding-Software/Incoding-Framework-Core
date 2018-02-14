using System;
using System.Linq.Expressions;
using Incoding.Extensions;
using Incoding.Mvc.MvcContrib.Extensions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Incoding.Mvc.MvcContrib.Incoding_Controls
{
    #region << Using >>

    #endregion

    public class IncFileControl<TModel, TProperty> : IncControlBase<TModel> where TModel : new()
    {
        #region Fields

        readonly IHtmlHelper<TModel> htmlHelper;

        #endregion

        #region Constructors

        public IncFileControl(IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> property)
        {
            this.htmlHelper = htmlHelper;
            this.attributes.Set("id", ReflectionExtensions.GetMemberNameAsHtmlId(property));
            this.attributes.Set("name", ReflectionExtensions.GetMemberName(property));
        }

        #endregion

        #region Properties

        public string Value { get; set; }

        #endregion

        public override IHtmlContent ToHtmlString()
        {
            return this.htmlHelper.Incoding().File(Value, GetAttributes());
        }
    }
}