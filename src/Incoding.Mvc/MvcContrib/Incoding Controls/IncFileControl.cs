using Incoding.Extensions;
using Incoding.Mvc.MvcContrib.Extensions;

namespace Incoding.Mvc.MvcContrib.Incoding_Controls
{
    #region << Using >>

    #endregion

    public class IncFileControl<TModel, TProperty> : IncControlBase
    {
        #region Fields

        readonly HtmlHelper<TModel> htmlHelper;

        #endregion

        #region Constructors

        public IncFileControl(HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> property)
        {
            this.htmlHelper = htmlHelper;
            this.attributes.Set("id", ReflectionExtensions.GetMemberNameAsHtmlId(property));
            this.attributes.Set("name", ReflectionExtensions.GetMemberName(property));
        }

        #endregion

        #region Properties

        public string Value { get; set; }

        #endregion

        public override MvcHtmlString ToHtmlString()
        {
            return HtmlExtensions.Incoding<TModel>(this.htmlHelper).File(Value, GetAttributes());
        }
    }
}