using System.Linq;
using System.Linq.Expressions;
using Incoding.Extensions;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;

namespace Incoding.Mvc.MvcContrib.Incoding_Controls
{
    #region << Using >>

    #endregion

    public class IncLabelControl : IncControlBase
    {
        #region Fields

        readonly IHtmlHelper htmlHelper;

        readonly string property;

        #endregion

        #region Constructors

        public IncLabelControl(IHtmlHelper htmlHelper, LambdaExpression property)
        {
            this.htmlHelper = htmlHelper;
            this.property = ReflectionExtensions.GetMemberName(property).Split(".".ToCharArray()).LastOrDefault();
        }

        #endregion

        #region Properties

        public string Name { get; set; }

        #endregion

        public override IHtmlContent ToHtmlString()
        {
            var tagBuilder = new TagBuilder("label");
            tagBuilder.Attributes.Add("for", TagBuilder.CreateSanitizedId(htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(property), "_"));

            var metadata = ExpressionMetadataProvider.FromStringExpression(property, htmlHelper.ViewData, htmlHelper.MetadataProvider);
            string innerText = Name ?? metadata.Metadata.DisplayName ?? property;
            tagBuilder.InnerHtml.Append(innerText);

            tagBuilder.MergeAttributes(attributes, true);
            tagBuilder.TagRenderMode = TagRenderMode.Normal;
            return new HtmlString(tagBuilder.ToString());
        }
    }
}