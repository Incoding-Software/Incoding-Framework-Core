using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text.Encodings.Web;
using Incoding.Core.Block.IoC;
using Incoding.Core.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
#if netcoreapp2_1
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
#endif

namespace Incoding.Web.MvcContrib
{
    #region << Using >>

    #endregion

    public class IncLabelControl<TModel> : IncControlBase<TModel>
    {
        #region Fields
        
        readonly string property;

        #endregion

        #region Constructors

        public IncLabelControl(IHtmlHelper<TModel> htmlHelper, LambdaExpression property) : base(htmlHelper)
        {
            this.property = ReflectionExtensions.GetMemberName(property).Split(".".ToCharArray()).LastOrDefault();
        }

        #endregion

        #region Properties

        public string Name { get; set; }

        #endregion
        
        public override void WriteTo(TextWriter writer, HtmlEncoder encoder)
        {
            var tagBuilder = new TagBuilder("label");
            tagBuilder.Attributes.Add("for", TagBuilder.CreateSanitizedId(htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(property), "_"));


#if netcoreapp2_1
                var metadata = ExpressionMetadataProvider.FromStringExpression(property, htmlHelper.ViewData, htmlHelper.MetadataProvider);
            string innerText = Name ?? metadata.Metadata.DisplayName ?? property;
#else
            var metadata = IoCFactory.Instance.TryResolve<ModelExpressionProvider>()
                .CreateModelExpression(htmlHelper.ViewData, property).ModelExplorer;
            string innerText = Name ?? metadata.Metadata.DisplayName ?? property;
#endif

            tagBuilder.InnerHtml.Append(innerText);

            tagBuilder.MergeAttributes(attributes, true);
            tagBuilder.TagRenderMode = TagRenderMode.Normal;
            tagBuilder.WriteTo(writer, encoder);
        }
    }
}