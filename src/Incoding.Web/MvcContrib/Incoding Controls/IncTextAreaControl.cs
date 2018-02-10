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

    public class IncTextAreaControl<TModel, TProperty> : IncControlBase
    {
        #region Fields

        readonly IHtmlHelper<TModel> htmlHelper;

        readonly Expression<Func<TModel, TProperty>> property;

        #endregion

        #region Constructors

        public IncTextAreaControl(IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> property)
        {
            this.htmlHelper = htmlHelper;
            this.property = property;
        }

        #endregion

        #region Properties

        /// <summary>
        ///     <see cref="HtmlAttribute.Cols" />
        /// </summary>
        public int Cols
        {
            set { this.attributes.Set(HtmlAttribute.Cols.ToStringLower(), value.ToString()); }
        }

        /// <summary>
        ///     <see cref="HtmlAttribute.Rows" />
        /// </summary>
        public int Rows
        {
            set { this.attributes.Set(HtmlAttribute.Rows.ToStringLower(), value.ToString()); }
        }

        public int MaxLenght { set { this.attributes.Set(HtmlAttribute.MaxLength.ToStringLower(), value); } }

        #endregion

        public override IHtmlContent ToHtmlString()
        {
            return this.htmlHelper.TextAreaFor(this.property, 5, 25, GetAttributes());
        }
    }
}