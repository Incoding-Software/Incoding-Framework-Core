using System;
using System.Linq.Expressions;
using Incoding.Core.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Incoding.Web.MvcContrib
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