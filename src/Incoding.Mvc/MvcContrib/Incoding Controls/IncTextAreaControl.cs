using Incoding.Extensions;
using Incoding.Mvc.MvcContrib.Incoding_Meta_Language.JqueryHelper.Primitive;

namespace Incoding.Mvc.MvcContrib.Incoding_Controls
{
    #region << Using >>

    #endregion

    public class IncTextAreaControl<TModel, TProperty> : IncControlBase
    {
        #region Fields

        readonly HtmlHelper<TModel> htmlHelper;

        readonly Expression<Func<TModel, TProperty>> property;

        #endregion

        #region Constructors

        public IncTextAreaControl(HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> property)
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

        public override MvcHtmlString ToHtmlString()
        {
            return this.htmlHelper.TextAreaFor(this.property, GetAttributes());
        }
    }
}