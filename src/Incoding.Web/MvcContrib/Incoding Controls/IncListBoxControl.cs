using System;
using System.Linq.Expressions;
using Incoding.Extensions;
using Incoding.Mvc.MvcContrib.Incoding_Meta_Language.JqueryHelper.Primitive;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Incoding.Mvc.MvcContrib.Incoding_Controls
{
    #region << Using >>

    #endregion

    public class IncListBoxControl<TModel, TProperty> : IncDropDownControl<TModel, TProperty>
    {
        #region Constructors

        public IncListBoxControl(IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> property)
                : base(htmlHelper, property)
        {
            this.attributes.Set(HtmlAttribute.Multiple.ToStringLower(), "multiple");
        }

        #endregion
    }
}