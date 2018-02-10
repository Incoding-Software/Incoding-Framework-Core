using Incoding.Extensions;
using Incoding.Mvc.MvcContrib.Incoding_Meta_Language.JqueryHelper.Primitive;

namespace Incoding.Mvc.MvcContrib.Incoding_Controls
{
    #region << Using >>

    #endregion

    public class IncListBoxControl<TModel, TProperty> : IncDropDownControl<TModel, TProperty>
    {
        #region Constructors

        public IncListBoxControl(HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> property)
                : base(htmlHelper, property)
        {
            this.attributes.Set(HtmlAttribute.Multiple.ToStringLower(), "multiple");
        }

        #endregion
    }
}