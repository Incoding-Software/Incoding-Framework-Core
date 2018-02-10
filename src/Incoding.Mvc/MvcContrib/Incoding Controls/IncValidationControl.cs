using Incoding.Extensions;

namespace Incoding.Mvc.MvcContrib.Incoding_Controls
{
    #region << Using >>

    #endregion

    public class IncValidationControl : IncControlBase
    {
        #region Fields

        readonly HtmlHelper htmlHelper;

        readonly string property;

        #endregion

        #region Constructors

        public IncValidationControl(HtmlHelper htmlHelper, LambdaExpression property)
        {
            this.htmlHelper = htmlHelper;
            this.property = ReflectionExtensions.GetMemberName(property);
        }

        #endregion

        public override MvcHtmlString ToHtmlString()
        {
            return this.htmlHelper.ValidationMessage(this.property, this.attributes);
        }
    }
}