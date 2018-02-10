using Incoding.Extensions;
using Incoding.Mvc.MvcContrib.HtmlHelper;
using Incoding.Mvc.MvcContrib.Incoding_Meta_Language.JqueryHelper.Primitive;

namespace Incoding.Mvc.MvcContrib.Incoding_Controls
{
    #region << Using >>

    #endregion

    public class IncCheckBoxControl<TModel, TProperty> : IncControlBase
    {

        #region Constructors

        public IncCheckBoxControl(HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> property)
        {
            this.htmlHelper = htmlHelper;
            this.property = property;
            this.Label = new IncLabelControl(htmlHelper, property);
        }

        #endregion

        public override MvcHtmlString ToHtmlString()
        {
            bool isChecked = this.attributes.ContainsKey(HtmlAttribute.Checked.ToStringLower());
            if (!isChecked)
            {
                var metadata = ModelMetadata.FromLambdaExpression(this.property, this.htmlHelper.ViewData);

                bool result;
                if (metadata.Model != null && bool.TryParse(metadata.Model.ToString(), out result))
                    isChecked = result;
            }

            var div = new TagBuilder(HtmlTag.Div.ToStringLower());
            div.AddCssClass(Mode == ModeOfCheckbox.Normal ? B.Checkbox.ToLocalization() : B.Checkbox_inline.ToLocalization());
            div.AddCssClass(GetAttributes().GetOrDefault(HtmlAttribute.Class.ToStringLower(), string.Empty).ToString());

            var spanAsLabel = new TagBuilder(HtmlTag.Span.ToStringLower());
            spanAsLabel.InnerHtml = this.Label.Name;
            var label = new TagBuilder(HtmlTag.Label.ToStringLower());
            label.InnerHtml = this.htmlHelper.CheckBox(ExpressionHelper.GetExpressionText(this.property), isChecked, GetAttributes()).ToHtmlString()
                              + new TagBuilder(HtmlTag.I.ToStringLower()).ToString(TagRenderMode.Normal)
                              + spanAsLabel.ToString(TagRenderMode.Normal);
            div.InnerHtml = label.ToString(TagRenderMode.Normal);

            return new MvcHtmlString(div.ToString(TagRenderMode.Normal));
        }

        #region Fields

        readonly HtmlHelper<TModel> htmlHelper;

        readonly Expression<Func<TModel, TProperty>> property;

        #endregion

        #region Properties

        public IncLabelControl Label { get; protected set; }

        public ModeOfCheckbox Mode { get; set; }

        #endregion
    }
}