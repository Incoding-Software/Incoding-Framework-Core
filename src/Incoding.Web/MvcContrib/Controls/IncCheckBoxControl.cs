using System;
using System.IO;
using System.Linq.Expressions;
using System.Text.Encodings.Web;
using Incoding.Core.Block.IoC;
using Incoding.Core.Extensions;
using Incoding.Web.Extensions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
#if netcoreapp3_1
using Microsoft.AspNetCore.Mvc.ViewFeatures;
#elif netcoreapp2_1
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
#endif

namespace Incoding.Web.MvcContrib
{
#region << Using >>

#endregion

    public class IncCheckBoxControl<TModel, TProperty> : IncControlBase<TModel>
    {

#region Constructors

        public IncCheckBoxControl(IHtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> property) : base(htmlHelper)
        {
            this.property = property;
            this.Label = new IncLabelControl<TModel>(htmlHelper, property);
        }

#endregion
        
#region Fields
        
        readonly Expression<Func<TModel, TProperty>> property;

#endregion

#region Properties

        public IncLabelControl<TModel> Label { get; protected set; }

        public ModeOfCheckbox Mode { get; set; }

#endregion

        public override void WriteTo(TextWriter writer, HtmlEncoder encoder)
        {
            bool isChecked = this.attributes.ContainsKey(HtmlAttribute.Checked.ToStringLower());
            if (!isChecked)
            {
#if netcoreapp3_1
                var metadata = IoCFactory.Instance.TryResolve<IModelExpressionProvider>()
                    .CreateModelExpression(this.htmlHelper.ViewData, this.property);
#elif netcoreapp2_1
                var metadata = ExpressionMetadataProvider.FromLambdaExpression(this.property, this.htmlHelper.ViewData, htmlHelper.MetadataProvider);
#endif
                bool result;
                if (metadata.Model != null && bool.TryParse(metadata.Model.ToString(), out result))
                    isChecked = result;
            }

            var div = new TagBuilder(HtmlTag.Div.ToStringLower());
            div.AddCssClass(Mode == ModeOfCheckbox.Normal ? B.Checkbox.ToLocalization() : B.Checkbox_inline.ToLocalization());
            div.AddCssClass(GetAttributes().GetOrDefault(HtmlAttribute.Class.ToStringLower(), string.Empty).ToString());

            var spanAsLabel = new TagBuilder(HtmlTag.Span.ToStringLower());
            spanAsLabel.InnerHtml.AppendHtml(this.Label.Name);
            var label = new TagBuilder(HtmlTag.Label.ToStringLower());
            label.InnerHtml.AppendHtml(this.htmlHelper.CheckBox(
#if netcoreapp3_1
                                           IoCFactory.Instance.TryResolve<ModelExpressionProvider>().GetExpressionText(this.property)
#elif netcoreapp2_1
                                           ExpressionHelper.GetExpressionText(this.property)
#endif
                                           , isChecked, GetAttributes()).HtmlContentToString()
                                        + new TagBuilder(HtmlTag.I.ToStringLower()).HtmlContentToString()
                                       + spanAsLabel.HtmlContentToString());
            div.InnerHtml.AppendHtml(label);
            div.WriteTo(writer, encoder);
        }
    }
}